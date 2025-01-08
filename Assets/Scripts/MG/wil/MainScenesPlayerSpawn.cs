using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool hasSpawned = false; // �ߺ� ���� ����

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon ������ ������� �ʾҽ��ϴ�. �κ�� �̵��մϴ�.");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�.");
            return;
        }

        Debug.Log("������ Ŭ���̾�Ʈ�� �ο��� Ȯ���Ͽ� �÷��̾ �����մϴ�.");
        CheckAndSpawnPlayers();
    }

    private void CheckAndSpawnPlayers()
    {
        if (PhotonNetwork.IsMasterClient) // ���� Ŭ���̾�Ʈ�� ������ Ŭ���̾�Ʈ���� Ȯ��
        {
            Debug.Log("������ Ŭ���̾�Ʈ�� �ο��� Ȯ�� ��...");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // �� �ο��� 2���� ���
            {
                Debug.Log("�� �ο��� 2���Դϴ�. �÷��̾ �����մϴ�.");
                SpawnPlayersForAll();
            }
            else
            {
                Debug.Log("�ο��� �����մϴ�. ��ٸ��ϴ�...");
            }
        }
    }

    private void SpawnPlayersForAll()
    {
        if (hasSpawned) return; // �ߺ� ���� ����

        // ��� Ŭ���̾�Ʈ���� �÷��̾ �����ϵ��� RPC ȣ��
        photonView.RPC("SpawnPlayer", RpcTarget.All);
        hasSpawned = true;
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber ������� ���� ��ġ ����
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");

        if (PhotonNetwork.IsMasterClient) // ������ Ŭ���̾�Ʈ�� �ο� Ȯ��
        {
            CheckAndSpawnPlayers();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"�÷��̾� {otherPlayer.NickName}��(��) �濡�� �������ϴ�.");
    }
}
