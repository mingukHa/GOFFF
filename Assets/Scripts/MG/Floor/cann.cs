using Photon.Pun;
using UnityEngine;

public class GrabSync : MonoBehaviourPun
{
    public void OnSelectEnter() //집으면 호출 되는 이벤트
    {
        if(!photonView.IsMine) //내꺼가 아니면
        {
            photonView.RequestOwnership(); //권한 요청
        }
        Debug.Log("오브젝트를 집었음");
    }
    public void OnTriggerEnter(Collider other) //콜라이더에 닿으면
    {
        if (!photonView.IsMine) //내꺼가 아니면
        {
            photonView.RequestOwnership(); //권한 요청
        }
        Debug.Log("오브젝트를 집었음");
    }
    public void Interact() //물건을 잡었을 때
    {
        // 소유권 요청
        if (!photonView.IsMine) //내 꺼가 아니면
        {
            photonView.RequestOwnership(); //권한 요청
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
