using UnityEngine;

public class CustomOVRManager : MonoBehaviour
{
    void Start()
    {
        // OVR Manager�� ��� �� �ʿ��� �κи� ȣ��
        OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;
    }

    void Update()
    {
        // OVR Input�� ó��
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Right Trigger Active");
        }
    }
}
