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
        if (player == null || playerPhotonView == null) return; // Null üũ
        StartCoroutine(RespawnCoroutine(player, playerPhotonView));
    }

    private IEnumerator RespawnCoroutine(GameObject player, PhotonView playerPhotonView)
    {
        // �÷��̾� ĳ���� ����
        playerPhotonView.RPC("SetPlayerActive", RpcTarget.All, false);
        yield return new WaitForSeconds(3f);

        // ���̺� ����Ʈ ����
        SaveManager.SavePoint respawnPoint = SaveManager.Instance.GetSavePoint(playerPhotonView.OwnerActorNr);

        // �÷��̾� ������
        playerPhotonView.RPC("RespawnPlayer", RpcTarget.All, respawnPoint.position, respawnPoint.rotation);
    }
}
