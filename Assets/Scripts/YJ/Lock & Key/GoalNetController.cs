using UnityEngine;
using Photon.Pun;

public class GoalNetController : MonoBehaviourPun
{
    public DoorController doorController; // 공이 골네트에 닿으면 열릴 문
    private Collider goalNetCollider; // 골네트에 부착된 콜라이더

    private void Start()
    {
        goalNetCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 충돌한 오브젝트가 'Ball' 태그를 가지고 있는지 확인
        if (collider.CompareTag("Ball"))
        {
            // RPC로 OpenLabDoor 메소드 호출
            if (doorController != null)
            {
                photonView.RPC("OpenLabDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {

        if (doorController != null)
        {
            // DoorController의 OpenDoor 메소드 호출하여 문 개방
            doorController.OpenDoor();
        }
    }
}
