using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviourPunCallbacks
{
    // Player의 Respawn Point를 저장하기 위한 Dictionary
    private Dictionary<int, Vector3> playerRespawnPoints = new Dictionary<int, Vector3>();

    // Game Over 상태 확인
    private bool isGameOver = false;

    // Singleton Instance
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RegisterInitialRespawnPoint(int playerId, Vector3 position)
    {
        if (!playerRespawnPoints.ContainsKey(playerId))
        {
            playerRespawnPoints[playerId] = position;
        }
    }

    public void UpdateRespawnPoint(int playerId, Vector3 newRespawnPoint)
    {
        if (playerRespawnPoints.ContainsKey(playerId))
        {
            playerRespawnPoints[playerId] = newRespawnPoint;
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return; // 중복 실행 방지
        isGameOver = true;

        // 모든 Player 비활성화
        foreach (var player in FindObjectsByType<PlayerManager>(FindObjectsSortMode.None))
        {
            player.DisablePlayer();
        }

        // 3초 후 Respawn
        StartCoroutine(RespawnAllPlayers());
    }

    private IEnumerator RespawnAllPlayers()
    {
        yield return new WaitForSeconds(3);

        foreach (var player in FindObjectsByType<PlayerManager>(FindObjectsSortMode.None))
        {
            int playerId = player.photonView.OwnerActorNr; // Photon Actor ID로 Player 식별
            if (playerRespawnPoints.TryGetValue(playerId, out Vector3 respawnPoint))
            {
                player.RespawnAt(respawnPoint);
            }
        }

        isGameOver = false;
    }
}
