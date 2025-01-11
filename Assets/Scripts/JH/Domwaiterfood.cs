using UnityEngine;
using Photon.Pun;

public class Domwaiterfood : MonoBehaviourPun
{
    [SerializeField] private DomwaiterOven triggerZone0;   // ��Ȳ�� �ڽ� [0]
    [SerializeField] private Transform targetPosition;  // ��Ȳ�� �ڽ� [1]

    public void OnButtonPressed()
    {
        if (photonView.IsMine) // �� ��ư�� ���� �÷��̾ RPC ȣ��
        {
            // RPC ȣ���� ���� ��� Ŭ���̾�Ʈ�� ������ ���� �̺�Ʈ�� ����
            photonView.RPC("SendItemsRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void SendItemsRPC()
    {
        // ��Ȳ�� �ڽ�[0]���� �������� ��Ȳ�� �ڽ�[1]���� �̵�
        triggerZone0.SendItemsToTarget(targetPosition);
    }
}