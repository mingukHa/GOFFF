using UnityEngine;
using Photon.Pun;

public class Domwaiterfood : MonoBehaviourPun
{
    [SerializeField] private DomwaiterOven triggerZone0;   // 박스[0]
    [SerializeField] private Transform targetPosition;  // 박스[1]

    public void OnButtonPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX");

        if (photonView.IsMine) // 이 버튼을 누른 플레이어만 RPC 호출
        {
            // RPC 호출을 통해 모든 클라이언트에 아이템 전송 이벤트를 전달
            photonView.RPC("SendItemsRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SendItemsRPC()
    {
        // 지하3층 오븐박스[0]에서 아이템을 지하2층 오븐박스[1]으로 이동
        triggerZone0.SendItemsToTarget(targetPosition);
    }
}