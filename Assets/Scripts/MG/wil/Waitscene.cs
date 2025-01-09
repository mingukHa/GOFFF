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
    private int readyPlayerCount = 0; // �غ� �Ϸ�� �÷��̾� ��

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
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            SpawnPlayer();
        }
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

        if (spawnPoint == null)
        {
            Debug.LogWarning($"���� ����Ʈ�� null�Դϴ�! Index: {playerIndex}. �⺻ ��ġ�� ����մϴ�.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true;

            // �÷��̾� ������Ʈ�� TagObject�� ����
            // PhotonNetwork.LocalPlayer.TagObject = player;
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }

        StartCoroutine(ReenableCollider(player));

        // ��� Ŭ���̾�Ʈ�� DontDestroyOnLoad ����
        photonView.RPC("SetDontDestroyOnLoad", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void SetDontDestroyOnLoad(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.gameObject != null)
        {
            DontDestroyOnLoad(targetView.gameObject);
            Debug.Log("��� PlayerHolder�� DontDestroyOnLoad ���� �Ϸ�.");
        }
    }

    private IEnumerator ReenableCollider(GameObject player)
    {
        if (player == null) yield break;

        Collider collider = player.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(1);
            collider.enabled = true;
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnButtonPressed()
    {
        photonView.RPC("PlayerReady", RpcTarget.AllBuffered); // ��� Ŭ���̾�Ʈ�� �÷��̾� �غ� ���� ����
        Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

        // ��� �÷��̾ �غ�Ǿ��� ��� ���� ������ ��ȯ
        if (readyPlayerCount == 2)
        {
            
            Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
            PhotonNetwork.LoadLevel("JHScenes3"); // ��ȯ�� �� �̸����� ����
            
        }
        else
        {
            return;
        }
        Debug.Log("��ư�� ���Ƚ��ϴ�");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
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
