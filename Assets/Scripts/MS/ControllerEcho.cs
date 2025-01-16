using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class ControllerEcho : MonoBehaviourPun
{
    private bool isBPressed = false;
    private bool wasBPressed = false; // 버튼이 마지막으로 눌린 상태 추적
    SimpleSonarShader_Parent par = null;
    private GameObject[] Player = null;

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
                // 플레이어가 둘뿐이니 둘만 계산
                // 자신의 플레이어 위치에만 링이 발생
                Player = GameObject.FindGameObjectsWithTag("Player");
                if (Player[0].GetPhotonView().IsMine)
                {
                    Vector3 point = Player[0].transform.position;
                    par.StartSonarRing(point, 1.2f, 0);
                    photonView.RPC("RPCSonarRing", RpcTarget.Others, point, Player[0]);
                    SoundManager.instance.SFXPlay("SuperSound_SFX", Player[0]);
                }
                else
                {
                    Vector3 point = Player[1].transform.position;
                    par.StartSonarRing(point, 1.2f, 0);
                    photonView.RPC("RPCSonarRing", RpcTarget.Others, point, Player[1]);
                    SoundManager.instance.SFXPlay("SuperSound_SFX", Player[1]);
                }
                wasBPressed = true;
            }
        }
        else
            wasBPressed = false;
    }

    [PunRPC]
    private void RPCSonarRing(Vector3 point, GameObject Player)
    {
        par.StartSonarRing(point, 0.7f, 0);
        SoundManager.instance.SFXPlay("SuperSound_SFX", Player);
    }
}
