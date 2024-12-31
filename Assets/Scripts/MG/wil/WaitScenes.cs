using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab; // �÷��̾� ������
    [SerializeField] private Transform[] spawnPoints; // ���� ��ġ �迭
    private bool hasSpawned = false; // �÷��̾ �����Ǿ����� Ȯ��

    private void Start()
    {
        // ���� ����Ʈ Ȯ��
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("���� ����Ʈ�� �������� �ʾҽ��ϴ�! �⺻ ��ġ�� ����մϴ�.");
            spawnPoints = new Transform[] { new GameObject("DefaultSpawn").transform };
            spawnPoints[0].position = Vector3.zero;
        }

        // Photon ���� ���� ���� Ȯ��
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon ������ ������� �ʾҽ��ϴ�. �κ�� �����մϴ�.");
            SceneManager.LoadScene("LobbyScene");
            return;
        }

        // �� ���� ���� Ȯ��
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�. �� ������ ��ٸ��ϴ�...");
            return;
        }

        // �濡 �̹� ������ ��� ��� �÷��̾� ����
        StartCoroutine(WaitForRoomReady());
    }

    public override void OnJoinedRoom()
    {
        // �� ������ �Ϸ�Ǿ��� �� ȣ��
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            SpawnPlayer();
        }
    }

    private IEnumerator WaitForRoomReady()
    {
        // Photon ������ �� ���°� �غ�� ������ ���
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        SpawnPlayer();
    }

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

        // ActorNumber�� ������� ���� ��ġ ����
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        if (spawnPoint == null)
        {
            Debug.LogWarning($"���� ����Ʈ�� null�Դϴ�! Index: {playerIndex}. �⺻ ��ġ�� ����մϴ�.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // ��Ʈ��ũ �� �÷��̾� ������ ����
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true; // ���� �Ϸ� �÷��� ����
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }

        // �浹 ���� ó�� (Collider ��Ȱ��ȭ �� Ȱ��ȭ)
        StartCoroutine(ReenableCollider(player));
    }

    private IEnumerator ReenableCollider(GameObject player)
    {
        if (player == null) yield break;

        Collider collider = player.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // �浹 ��Ȱ��ȭ
            yield return new WaitForSeconds(1); // 1�� �� �ٽ� Ȱ��ȭ
            collider.enabled = true;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");

        // �� ���� UI ������Ʈ
        UpdatePlayerListUI();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"�÷��̾� {otherPlayer.NickName}��(��) �濡�� �������ϴ�.");

        // �� ���� UI ������Ʈ
        UpdatePlayerListUI();
    }

    private void UpdatePlayerListUI()
    {
        Debug.Log("���� ���� �÷��̾� ���:");
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"�÷��̾�: {player.NickName}");
        }
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($"Photon ���� ���� ����: {cause}. �κ�� �����մϴ�.");
        SceneManager.LoadScene("LobbyScene");
    }
}
