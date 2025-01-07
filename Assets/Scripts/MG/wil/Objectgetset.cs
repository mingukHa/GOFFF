using Photon.Pun;
using UnityEngine;

public class SharedObject : MonoBehaviourPun
{
    public void UpdatePosition(Vector3 newPosition)
    {
        // ��� Ŭ���̾�Ʈ�� ������Ʈ ��ġ ������Ʈ ��û
        photonView.RPC("RPC_UpdatePosition", RpcTarget.All, newPosition);
    }

    [PunRPC]
    public void RPC_UpdatePosition(Vector3 newPosition)
    {
        // ��� Ŭ���̾�Ʈ���� ������Ʈ ��ġ ����
        transform.position = newPosition;
    }
}
