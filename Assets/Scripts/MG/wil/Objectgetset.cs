using Photon.Pun;
using UnityEngine;

public class SharedObject : MonoBehaviourPun
{
    public void UpdatePosition(Vector3 newPosition)
    {
        // 모든 클라이언트에 오브젝트 위치 업데이트 요청
        photonView.RPC("RPC_UpdatePosition", RpcTarget.All, newPosition);
    }

    [PunRPC]
    public void RPC_UpdatePosition(Vector3 newPosition)
    {
        // 모든 클라이언트에서 오브젝트 위치 갱신
        transform.position = newPosition;
    }
}
