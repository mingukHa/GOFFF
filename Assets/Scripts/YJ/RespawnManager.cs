using UnityEngine;
using System.Collections;
using Photon.Pun;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HandlePlayerRespawn(GameObject player, PhotonView playerPhotonView)
    {
        if (player == null || playerPhotonView == null) return; // Null 체크
        StartCoroutine(RespawnCoroutine(player, playerPhotonView));
    }

    private IEnumerator RespawnCoroutine(GameObject player, PhotonView playerPhotonView)
    {
        // 플레이어 캐릭터 제거
        playerPhotonView.RPC("SetPlayerActive", RpcTarget.All, false);
        yield return new WaitForSeconds(3f);

        // 세이브 포인트 생성
        SaveManager.SavePoint respawnPoint = SaveManager.Instance.GetSavePoint(playerPhotonView.OwnerActorNr);

        // 플레이어 리스폰
        playerPhotonView.RPC("RespawnPlayer", RpcTarget.All, respawnPoint.position, respawnPoint.rotation);
    }
}
