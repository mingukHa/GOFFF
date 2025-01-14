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
        Debug.Log($"{other}공 충돌함");

        // 충돌한 오브젝트가 'Ball' 태그를 가지고 있는지 확인
        if (other.CompareTag("Ball"))
        {
            
            // DoorController의 OpenDoor 메서드 호출 및 동기화
            if (doorController != null)
            {
                OpenLabDoor();
                photonView.RPC("OpenLabDoor", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {
        doorController.OpenDoor();
    }
}
