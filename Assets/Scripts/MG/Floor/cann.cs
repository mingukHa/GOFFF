using Photon.Pun;
using UnityEngine;

public class GrabSync : MonoBehaviourPun
{
    public void OnSelectEnter()
    {
        if(!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("������Ʈ�� ������");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("������Ʈ�� ������");
    }
    public void Interact()
    {
        // ������ ��û
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
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
