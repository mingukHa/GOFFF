using Photon.Pun;
using UnityEngine;

public class GrabSync : MonoBehaviourPun
{
    public void OnSelectEnter() //������ ȣ�� �Ǵ� �̺�Ʈ
    {
        if(!photonView.IsMine) //������ �ƴϸ�
        {
            photonView.RequestOwnership(); //���� ��û
        }
        Debug.Log("������Ʈ�� ������");
    }
    public void OnTriggerEnter(Collider other) //�ݶ��̴��� ������
    {
        if (!photonView.IsMine) //������ �ƴϸ�
        {
            photonView.RequestOwnership(); //���� ��û
        }
        Debug.Log("������Ʈ�� ������");
    }
    public void Interact() //������ ����� ��
    {
        // ������ ��û
        if (!photonView.IsMine) //�� ���� �ƴϸ�
        {
            photonView.RequestOwnership(); //���� ��û
        }

        // ��ȣ�ۿ� ���� ����ȭ
        photonView.RPC("RpcHandleInteraction", RpcTarget.All, true);
    }
    [PunRPC]
    public void RpcHandleInteraction(bool interactionState)
    {
        Debug.Log($"��ȣ�ۿ� ����: {interactionState}");
    }
}
