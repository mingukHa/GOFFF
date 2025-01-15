using UnityEngine;
using Photon.Pun;

public class Domwaiterfood : MonoBehaviourPun
{
    [SerializeField] private DomwaiterOven triggerZone0;   // 주황색 박스 [0]
    [SerializeField] private Transform targetPosition;  // 주황색 박스 [1]
  

    

    public void OnButtonPressed()
    {
        
        /*  멀티가 아닌 상황에서 로컬 로직
        
        // 주황색 박스[0]dptj 충돌된 모든 오브젝트 가져오기
        var objectsToMove = triggerZone0.GetObjectsInZone();

        // 모든 오브젝트를 주황색 박스[1] 위치로 이동
        foreach (GameObject obj in objectsToMove)
        {
            obj.transform.position = targetPosition.position;
        }

        */

        //if (photonView.IsMine) // 이 버튼을 누른 플레이어만 아이템 전송 요청을 보냄
        //{
        //    // 주황색 박스[0]에서 아이템을 전송
            triggerZone0.SendItemsToTarget(targetPosition);
        //}
    }
}