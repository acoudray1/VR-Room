﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerAndHandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.TryInitialize();
    }

    void TryInitialize() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(this.controllerCharacteristics, devices);

        if (devices.Count > 0) {
            this.targetDevice = devices[0];
            GameObject prefab = this.controllerPrefabs.Find(controller => controller.name == this.targetDevice.name);
            if (prefab) {
                this.spawnedController = Instantiate(prefab, transform);
            }
        } else {
            Debug.Log("did not find corresponding controller model");
            this.spawnedController = Instantiate(this.controllerPrefabs[0], transform);
        }

        this.spawnedHandModel = Instantiate(this.handModelPrefab, transform);
        this.handAnimator = this.spawnedHandModel.GetComponent<Animator>();
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
            if (this.showController) {
                this.spawnedHandModel.SetActive(false);
                this.spawnedController.SetActive(true);
            } else {
                this.spawnedHandModel.SetActive(true);
                this.spawnedController.SetActive(false);
                this.UpdateHandAnimation();
            }
        }

        /*
        this.targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue) {
            Debug.Log("Pressing primary button");
        }

        this.targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f) {
            Debug.Log("Trigger pressed: " + triggerValue);
        }

        this.targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero) {
            Debug.Log("Primary touchpad: " + primary2DAxisValue);
        }
        */
    }
}
