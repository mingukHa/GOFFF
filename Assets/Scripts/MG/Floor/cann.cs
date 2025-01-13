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
        Debug.Log("오브젝트를 집었음");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("오브젝트를 집었음");
    }
    public void Interact()
    {
        // 소유권 요청
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }

        // 상호작용 상태 동기화
        photonView.RPC("RpcHandleInteraction", RpcTarget.All, true);
    }
    [PunRPC]
    public void RpcHandleInteraction(bool interactionState)
    {
        Debug.Log($"상호작용 상태: {interactionState}");
    }
}
