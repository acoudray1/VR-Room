using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.leftTeleportRay) {
            this.leftTeleportRay.gameObject.SetActive(this.CheckIfActivated(this.leftTeleportRay));
        }

        if (this.rightTeleportRay) {
            this.rightTeleportRay.gameObject.SetActive(this.CheckIfActivated(this.rightTeleportRay));
        }
    }

    public bool CheckIfActivated(XRController controller) {
        InputHelpers.IsPressed(controller.inputDevice, this.teleportActivationButton, out bool isActivated, this.activationThreshold);

        return isActivated;
    }
}
