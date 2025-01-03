using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    private bool isDisabled = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // 처음 생성 시 초기 Respawn Point 등록
            RespawnManager.Instance.RegisterInitialRespawnPoint(photonView.OwnerActorNr, transform.position);
            Debug.Log("초기 리스폰 포인트 생성 완료");
        }
    }

    private void Update()
    {
        // Debug용 Instant Death
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            RespawnManager.Instance.TriggerGameOver();
        }
    }

    public void DisablePlayer()
    {
        if (isDisabled) return;

        isDisabled = true;
        gameObject.SetActive(false); // Player 비활성화
    }

    public void RespawnAt(Vector3 respawnPoint)
    {
        transform.position = respawnPoint;
        gameObject.SetActive(true); // Player 활성화
        isDisabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 특정 장소를 지나가면 Respawn Point 갱신
        if (other.CompareTag("RespawnPoint") && photonView.IsMine)
        {
            RespawnManager.Instance.UpdateRespawnPoint(photonView.OwnerActorNr, transform.position);
        }
    }
}
