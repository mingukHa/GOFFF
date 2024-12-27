using UnityEngine;

public class CustomOVRManager : MonoBehaviour
{
    void Start()
    {
        // OVR Manager의 기능 중 필요한 부분만 호출
        OVRManager.instance.trackingOriginType = OVRManager.TrackingOrigin.FloorLevel;
    }

    void Update()
    {
        // OVR Input만 처리
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            Debug.Log("Right Trigger Active");
        }
    }
}
