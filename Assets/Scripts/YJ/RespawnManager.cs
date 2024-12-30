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
        if (player == null || playerPhotonView == null) return; // Null 체크 추가
        StartCoroutine(RespawnCoroutine(player, playerPhotonView));
    }

    private IEnumerator RespawnCoroutine(GameObject player, PhotonView playerPhotonView)
    {
        // Disable the player on all clients
        playerPhotonView.RPC("SetPlayerActive", RpcTarget.All, false);
        yield return new WaitForSeconds(3f);

        // Get the respawn point
        SaveManager.SavePoint respawnPoint = SaveManager.Instance.GetSavePoint(playerPhotonView.OwnerActorNr);

        // Respawn player on all clients
        playerPhotonView.RPC("RespawnPlayer", RpcTarget.All, respawnPoint.position, respawnPoint.rotation);
    }
}
