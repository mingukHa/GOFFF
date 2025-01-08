using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private GameObject playerInstance; // �÷��̾� ��ü ����
    private bool hasSpawned = false;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon ������ ������ �� �Ǿ����ϴ�. �κ�� �̵��մϴ�.");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�.");
            return;
        }

        SpawnPlayer();

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoints == null)
        {
            Debug.LogError("Player Prefab �Ǵ� Spawn Points�� �������� �ʾҽ��ϴ�.");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        playerInstance = PhotonNetwork.Instantiate(playerPrefab[playerIndex % playerPrefab.Length].name, spawnPoint.position, spawnPoint.rotation);
        DontDestroyOnLoad(playerInstance); // �� ��ȯ �� ��ü ����

        hasSpawned = true;
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} �÷��̾ �����Ǿ����ϴ�.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"�� �ε� �Ϸ�: {scene.name}. ����ȭ ����.");

        // �� �ε� �� RPC�� ���� ����ȭ
        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", RpcTarget.AllBuffered, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }

    [PunRPC]
    private void SyncPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerInstance == null)
        {
            Debug.Log("�÷��̾� ��ü�� �����Ƿ� ����ȭ ����.");
            return;
        }

        // ��ü ��ġ �� ȸ�� ����ȭ
        playerInstance.transform.position = position;
        playerInstance.transform.rotation = rotation;

        Debug.Log("�÷��̾� ���� ����ȭ �Ϸ�.");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");

        // ���ο� �÷��̾�� ����ȭ ���� ����
        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", newPlayer, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }
}
