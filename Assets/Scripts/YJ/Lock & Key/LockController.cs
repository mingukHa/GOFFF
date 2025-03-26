using UnityEngine;
using Photon.Pun;

public class LockController : MonoBehaviourPun
{
    public DoorManager doorController; // 열쇠가 자물쇠에 닿으면 열릴 문
    private Collider lockCollider; // 자물쇠에 부착된 콜라이더

    private void Start()
    {
        lockCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // 충돌한 오브젝트가 'Key' 태그를 가지고 있는지 확인
        if (collider.CompareTag("Key"))
        {
            // RPC로 OpenDoorAndDestroy 메소드 호출
            if (doorController != null)
            {
                photonView.RPC("OpenDoorAndDestroy", RpcTarget.All, collider.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    private void OpenDoorAndDestroy(int keyViewID)
    {
        if (doorController != null)
        {
            // DoorController의 OpenDoor 메소드 호출하여 문 개방
            doorController.OpenDoor();
        }

        // 열쇠 오브젝트 제거
        PhotonView keyPhotonView = PhotonView.Find(keyViewID);
        if (keyPhotonView != null)
        {
            Destroy(keyPhotonView.gameObject);
        }

        // 자물쇠 오브젝트 제거
        Destroy(gameObject);
    }
}
