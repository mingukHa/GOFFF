using UnityEngine;
using Photon.Pun;

public class WalkieTalkie : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");

            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            // 로컬 플레이어만 처리
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // 무전기 생성
                GameObject walkieTalkie =
                    PhotonNetwork.Instantiate(
                    "WalkieTalkie(1P)", // 프리팹 이름 (Resources 폴더에 있어야 함)
                    other.transform.position + Vector3.up, // 플레이어 위에 생성
                    Quaternion.identity // 기본 회전값
                    );

                // 무전기를 획득하고 저장소에 좌표를 등록
                photonView.RPC("RegisterSavePointRPC", RpcTarget.All, transform.position, transform.rotation);

                // 무전기를 비활성화하거나 파괴 (옵션)
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    private void RegisterSavePointRPC(Vector3 position, Quaternion rotation)
    {
        SaveManager.Instance.RegisterSavePoint(photonView.OwnerActorNr, position, rotation);
    }
}