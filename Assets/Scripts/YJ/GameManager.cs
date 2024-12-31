using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3[] spawnPositions;    // �÷��̾��� ���� ��ġ �迭
    public Quaternion[] spawnRotations; // �÷��̾��� ���� ȸ���� �迭

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // Photon�� ActorNumber�� 1���� ����
        Vector3 spawnPosition = spawnPositions[playerIndex % spawnPositions.Length];    // �迭 �ε��� �ʰ� ����
        Quaternion spawnRotation = spawnRotations[playerIndex & spawnRotations.Length];

        PhotonNetwork.Instantiate("PlayerHolder", spawnPosition, spawnRotation);
    }
}
