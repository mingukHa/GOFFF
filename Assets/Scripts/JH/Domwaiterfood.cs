using UnityEngine;
using Photon.Pun;

public class Domwaiterfood : MonoBehaviourPun
{
    [SerializeField] private DomwaiterOven triggerZone0;   // �ڽ�[0]
    [SerializeField] private Transform targetPosition;  // �ڽ�[1]

    public void OnButtonPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX");

        if (photonView.IsMine) // �� ��ư�� ���� �÷��̾ RPC ȣ��
        {
            // RPC ȣ���� ���� ��� Ŭ���̾�Ʈ�� ������ ���� �̺�Ʈ�� ����
            photonView.RPC("SendItemsRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SendItemsRPC()
    {
        // ����3�� ����ڽ�[0]���� �������� ����2�� ����ڽ�[1]���� �̵�
        triggerZone0.SendItemsToTarget(targetPosition);
    }
}