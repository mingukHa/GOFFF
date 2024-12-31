using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3[] spawnPositions;    // 플레이어의 시작 위치 배열
    public Quaternion[] spawnRotations; // 플레이어의 시작 회전값 배열

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Photon의 ActorNumber는 1부터 시작
        Vector3 spawnPosition = spawnPositions[playerIndex % spawnPositions.Length];    // 배열 인덱스 초과 방지
        Quaternion spawnRotation = spawnRotations[playerIndex & spawnRotations.Length];

        PhotonNetwork.Instantiate("PlayerHolder", spawnPosition, spawnRotation);
    }
}
