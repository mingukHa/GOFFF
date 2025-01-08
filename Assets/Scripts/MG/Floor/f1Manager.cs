using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class f1Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; // ������ �迭
    [SerializeField] private Transform[] spawnPoints;   // ���� ��ġ
    private bool hasSpawned = false;                   // ���� Ȯ�ο�

    private void Start()
    {
        // ���� ��Ʈ��ũ ���� Ȯ��
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("���� ������ ������ �� �Ǿ��� �κ�� �̵�");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        // �� ���� ���� Ȯ��
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�. �� ������ ��ٸ��ϴ�...");
            return;
        }

        // ������ �Ϸ�Ǹ� �÷��̾� ���� ���
        Debug.Log("���� �濡 �����߽��ϴ�. �ٸ� �÷��̾ ��ٸ��ϴ�...");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // �ٸ� �÷��̾ �濡 �������� �� ȣ��
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�. ���� �÷��̾� ��: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // �濡 �÷��̾ 2���� �Ǹ� ����
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("��� �÷��̾ �����߽��ϴ�. �÷��̾ �����մϴ�.");
            SpawnPlayer();
        }
    }

   public override void OnJoinedRoom()
   {
       // �ڽ��� �濡 �������� �� ȣ��
       Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}. ���� �÷��̾� ��: {PhotonNetwork.CurrentRoom.PlayerCount}");

       // �̹� �濡 2���� �ִٸ� ���� (�ٸ� Ŭ���̾�Ʈ�� ���� ���� �ִ� ���)
       if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
       {
           Debug.Log("��� �÷��̾ �����߽��ϴ�. �÷��̾ �����մϴ�.");
           SpawnPlayer();
       }
   }

    private void SpawnPlayer()
    {
        // �÷��̾� ������ �迭�� �������� ���� ���
        if (playerPrefab == null || playerPrefab.Length == 0)
        {
            Debug.LogError("Player Prefab�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // ���� ����Ʈ �迭�� �������� ���� ���
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // �÷��̾��� ���� ActorNumber�� ������� ���� ��ġ ���
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        // ��Ʈ��ũ �󿡼� �÷��̾� ����
        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true; // �ߺ� ���� ����
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
    }
}
