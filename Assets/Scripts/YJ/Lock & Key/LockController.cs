using UnityEngine;
using Photon.Pun;

public class LockController : MonoBehaviourPun
{
    // DoorController를 참조할 필드
    public DoorController doorController;
    private Collider lockColliders;

    private void Start()
    {
        lockColliders = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 'Key' 태그를 가지고 있는지 확인
        if (other.CompareTag("Key"))
        {
            // DoorController의 OpenDoor 메서드 호출 및 동기화
            if (doorController != null)
            {
                photonView.RPC("OpenDoorAndDestroy", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenDoorAndDestroy()
    {
        // DoorController의 OpenDoor 메서드 호출
        if (doorController != null)
        {
            doorController.OpenDoor();
        }

        // Lock 오브젝트 제거
        Destroy(gameObject);
    }
}
