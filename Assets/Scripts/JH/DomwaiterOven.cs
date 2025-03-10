using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DomwaiterOven : MonoBehaviourPun
{
    public List<GameObject> objectsInZone = new List<GameObject>(); //충돌된 오브젝트들 관리 리스트

    private void OnTriggerEnter(Collider other)
    {
        // Trigger Zone에 들어온 오브젝트를 리스트에 추가
        if (!objectsInZone.Contains(other.gameObject))
        {
            objectsInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Trigger Zone에서 나간 오브젝트들 리스트에서 제거
        if (objectsInZone.Contains(other.gameObject))
        {
            objectsInZone.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetObjectsInZone()
    {
        // 현재 Trigger Zone에 있는 오브젝트 리스트 반환
        return objectsInZone;
    }

    // 아이템 전송 메서드
    public void SendItemsToTarget(Transform targetPosition)
    {
        foreach (GameObject obj in objectsInZone)
        {
            //PhotonView를 통해 모든 플레이어에게 RPC 호출
            photonView.RPC("MoveItemToTarget", RpcTarget.All,
                obj.GetPhotonView().ViewID, targetPosition.position);
        }

        objectsInZone.Clear();  //박스[0]에서 아이템 비우기
    }

    [PunRPC]
    private void MoveItemToTarget(int viewID, Vector3 targetPosition)
    {
        PhotonView itemView = PhotonView.Find(viewID);
        if (itemView != null)
        {
            itemView.transform.position = targetPosition;
        }
    }
}
