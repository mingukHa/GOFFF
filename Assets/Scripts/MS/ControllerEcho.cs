using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class ControllerEcho : MonoBehaviourPun
{
    private bool isBPressed = false;
    private bool wasBPressed = false; // 버튼이 마지막으로 눌린 상태 추적
    SimpleSonarShader_Parent par = null;

    private void Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();
    }
    private void Update()
    {
        // 오른손 컨트롤러의 입력 상태 가져오기
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Secondary Button (B 버튼) 상태 읽기
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
        {
            Debug.Log("파동 입력을 받고 있음");
            if (!wasBPressed)
            {
                par.StartSonarRing(transform.position, 0.7f, 0);
                wasBPressed = true;
            }
        }
        else
            wasBPressed = false;
    }

    [PunRPC]
    private void RPCSonarRing(Vector3 point)
    {
        par.StartSonarRing(point, 0.7f, 0);
    }
}
