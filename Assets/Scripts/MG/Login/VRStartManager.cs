using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections;

public class VRManagerStartManager : MonoBehaviour //VR강제 활성화 코드
{
    private void Start()
    {
        // OpenXR VR 환경 강제 활성화
        StartCoroutine(EnableVR());
    }

    private IEnumerator EnableVR()
    {
        Debug.Log("Initializing VR...");
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        yield return XRGeneralSettings.Instance.Manager.activeLoader;

        if (XRSettings.isDeviceActive)
        {
            Debug.Log($"XR Device Loaded: {XRSettings.loadedDeviceName}");
            XRSettings.enabled = true;
        }
        else
        {
            Debug.LogWarning("XR Device failed to load.");
        }
    }
}

