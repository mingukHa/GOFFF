using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class ControllerEcho : MonoBehaviourPun
{
    private bool isBPressed = false;
    private bool wasBPressed = false; // ��ư�� ���������� ���� ���� ����
    SimpleSonarShader_Parent par = null;
    private GameObject[] Player = null;

    private void Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();
    }
    private void Update()
    {
        // ������ ��Ʈ�ѷ��� �Է� ���� ��������
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Secondary Button (B ��ư) ���� �б�
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
        {
            Debug.Log("�ĵ� �Է��� �ް� ����");
            if (!wasBPressed)
            {
                // �÷��̾ �ѻ��̴� �Ѹ� ���
                // �ڽ��� �÷��̾� ��ġ���� ���� �߻�
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
