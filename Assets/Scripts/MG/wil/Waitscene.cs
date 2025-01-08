using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;

    private bool hasSpawned = false;
    private int readyPlayerCount = 0;

    private GameObject playerInstance; // �÷��̾� �ν��Ͻ� ����

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

        StartCoroutine(WaitForRoomReady());

        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator WaitForRoomReady()
    {
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

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        playerInstance = PhotonNetwork.Instantiate(playerPrefab[playerIndex % playerPrefab.Length].name, spawnPoint.position, spawnPoint.rotation);

        if (playerInstance != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true;

            DontDestroyOnLoad(playerInstance);
            Debug.Log($"{playerInstance.name} �ı����� ���� �Ϸ�");
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"�� �ε� �Ϸ�: {scene.name}. �÷��̾� ����ȭ ����.");

        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", RpcTarget.OthersBuffered, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }

    [PunRPC]
    private void SyncPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerInstance == null)
        {
            // ���ÿ��� �÷��̾ �����
            playerInstance = PhotonNetwork.Instantiate(playerPrefab[0].name, position, rotation);
            DontDestroyOnLoad(playerInstance);
            Debug.Log("����ȭ�� �÷��̾� ���� �Ϸ�.");
        }
        else
        {
            // ���� �÷��̾��� ��ġ �� ȸ���� ����ȭ
            playerInstance.transform.position = position;
            playerInstance.transform.rotation = rotation;
            Debug.Log("�÷��̾� ��ġ �� ȸ�� ����ȭ �Ϸ�.");
        }
    }

    public void OnButtonPressed()
    {
        photonView.RPC("PlayerReady", RpcTarget.AllBuffered);
        Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

        if (readyPlayerCount >= 2)
        {
            Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
            PhotonNetwork.LoadLevel("JHScenes3");
        }
    }

    [PunRPC]
    public void PlayerReady()
    {
        readyPlayerCount++;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");
    }
}
