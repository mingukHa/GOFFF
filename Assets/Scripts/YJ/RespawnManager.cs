using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class RespawnManager : MonoBehaviourPunCallbacks
{
    // Player�� Respawn Point�� �����ϱ� ���� Dictionary
    private Dictionary<int, Vector3> playerRespawnPoints = new Dictionary<int, Vector3>();

    // Game Over ���� Ȯ��
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
        if (isGameOver) return; // �ߺ� ���� ����
        isGameOver = true;

        // ��� Player ��Ȱ��ȭ
        foreach (var player in FindObjectsByType<PlayerManager>(FindObjectsSortMode.None))
        {
            player.DisablePlayer();
        }

        // 3�� �� Respawn
        StartCoroutine(RespawnAllPlayers());
    }

    private IEnumerator RespawnAllPlayers()
    {
        yield return new WaitForSeconds(3);

        foreach (var player in FindObjectsByType<PlayerManager>(FindObjectsSortMode.None))
        {
            int playerId = player.photonView.OwnerActorNr; // Photon Actor ID�� Player �ĺ�
            if (playerRespawnPoints.TryGetValue(playerId, out Vector3 respawnPoint))
            {
                player.RespawnAt(respawnPoint);
            }
        }

        isGameOver = false;
    }
}
