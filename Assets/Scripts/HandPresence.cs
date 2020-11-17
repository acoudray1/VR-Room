using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModelPrefab;
    private InputDevice targetDevice;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.TryInitialize();
        
        this.spawnedHandModel = Instantiate(this.handModelPrefab, transform);
        this.handAnimator = this.spawnedHandModel.GetComponent<Animator>();
    }

    void TryInitialize() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(this.controllerCharacteristics, devices);

        if (devices.Count > 0) {
            this.targetDevice = devices[0];
        }
    }

    void UpdateHandAnimation() {
        if (this.targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)) {
            this.handAnimator.SetFloat("Trigger", triggerValue);
        } else {
            this.handAnimator.SetFloat("Trigger", 0);
        }

        if (this.targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)) {
            this.handAnimator.SetFloat("Grip", gripValue);
        } else {
            this.handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.targetDevice.isValid) {
            this.TryInitialize();
        } else {
            this.spawnedHandModel.SetActive(true);
            this.UpdateHandAnimation();
        }
    }
}
