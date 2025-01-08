using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private bool hasSpawned = false;
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

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true;
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }

        StartCoroutine(ReenableCollider(player));
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
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");
    }
}
