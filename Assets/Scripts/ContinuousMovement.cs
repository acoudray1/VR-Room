using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    private Vector2 inputAxis;
    private CharacterController character;
    private XRRig rig;

    // Start is called before the first frame update
    void Start()
    {
        this.character = GetComponent<CharacterController>();
        this.rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(this.inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        this.CapsuleFollowHeadset();
        
        Quaternion headYaw = Quaternion.Euler(0, this.rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(this.inputAxis.x, 0, this.inputAxis.y);

        this.character.Move(direction * Time.fixedDeltaTime * this.speed);

        if (this.CheckIfGrounded()) {
            this.fallingSpeed = 0;
        } else {
            this.fallingSpeed += this.gravity * Time.fixedDeltaTime;
            this.character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        }
    }

    bool CheckIfGrounded() {
        Vector3 rayStart = transform.TransformPoint(this.character.center);
        float rayLength = this.character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, this.groundLayer);
        return hasHit;
    }

    void CapsuleFollowHeadset() {
        this.character.height = this.rig.cameraInRigSpaceHeight + this.additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(this.rig.cameraGameObject.transform.position);
        this.character.center = new Vector3(capsuleCenter.x, this.character.height/2 + this.character.skinWidth, capsuleCenter.z);
    }
}
