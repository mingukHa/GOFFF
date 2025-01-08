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
                // B 버튼이 눌렸을 때 동작
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                if (Physics.Raycast(ray, out hit))
                {
                    par.StartSonarRing(hit.point, 1.4f, 0);
                    photonView.RPC("RPCSonarRing", RpcTarget.Others, hit.point);
                    Debug.Log($"{hit.point}입니다");
                }
                wasBPressed = true;
            }
        }
        else
            wasBPressed = false;
    }

    [PunRPC]
    private void RPCSonarRing(Vector3 point)
    {
        par.StartSonarRing(point, 1.4f, 0);
    }
}
