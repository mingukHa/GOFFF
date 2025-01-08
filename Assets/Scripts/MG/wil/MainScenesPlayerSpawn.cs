using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; // ������ �迭
    [SerializeField] private Transform[] spawnPoints;   // ���� ��ġ
    private bool hasSpawned = false;                   // ���� Ȯ�ο�

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("���� ������ ������ �� �Ǿ��� �κ�� �̵�");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�. �� ������ ��ٸ��ϴ�...");
            return;
        }

        Debug.Log("���� �濡 �����߽��ϴ�. �ٸ� �÷��̾ ��ٸ��ϴ�...");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�. ���� �÷��̾� ��: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("��� �÷��̾ �����߽��ϴ�. �÷��̾ �����մϴ�.");
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}. ���� �÷��̾� ��: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("��� �÷��̾ �����߽��ϴ�. �÷��̾ �����մϴ�.");
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || playerPrefab.Length == 0)
        {
            Debug.LogError("Player Prefab�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // ������ �� ���� ����Ʈ �ε��� ���
        GameObject selectedPrefab = playerPrefab[playerIndex % playerPrefab.Length];
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        GameObject player = PhotonNetwork.Instantiate(selectedPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true;
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
    }
}
