using UnityEngine;
using Photon.Pun;

public class GoalNetController : MonoBehaviourPun
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
        Debug.Log("공 충돌함");

        // 충돌한 오브젝트가 'Ball' 태그를 가지고 있는지 확인
        if (other.CompareTag("Ball"))
        {
            // DoorController의 OpenDoor 메서드 호출 및 동기화
            if (doorController != null)
            {
                photonView.RPC("OpenLabDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {
        // DoorController의 OpenDoor 메서드 호출
        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}
