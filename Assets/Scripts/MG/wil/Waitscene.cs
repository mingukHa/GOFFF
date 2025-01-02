using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class Waitscene : MonoBehaviourPunCallbacks
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
    [PunRPC]
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        photonView.RPC("NotifyPlayerSpawned", newPlayer, PhotonNetwork.LocalPlayer.NickName);
        Debug.Log($"�÷��̾� {newPlayer.NickName}��(��) �濡 �����߽��ϴ�.");
    }


}
