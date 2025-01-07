using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScenesPlayerSpawn : MonoBehaviourPun
{
    [SerializeField] private GameObject playerPrefab; // �÷��̾� ������
    [SerializeField] private Transform[] spawnPoints; // ���� ��ġ �迭
    private bool hasSpawned = false;
    private bool isReinitializing = false;

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

        // �� ������ ��ٸ� �� �÷��̾� ����
        StartCoroutine(WaitForRoomReady());
    }

    private IEnumerator WaitForRoomReady()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        // ActorNumber�� ������� ������ ����
        float delay = (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 2f;
        Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName} ���� ������: {delay}��");
        yield return new WaitForSeconds(delay); // ������ �� ����

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

        // ActorNumber�� ������� ���� ����Ʈ ����
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        if (spawnPoint == null)
        {
            Debug.LogWarning($"���� ����Ʈ�� null�Դϴ�! Index: {playerIndex}. �⺻ ��ġ�� ����մϴ�.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // �÷��̾� ����
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

    // ���ʱ�ȭ ���
    public void ReinitializePlayer()
    {
        if (isReinitializing) return; // �ߺ� ���� ����

        Debug.Log("�÷��̾� ���ʱ�ȭ ����...");
        isReinitializing = true;

        // ���� �÷��̾� ������Ʈ ����
        RemoveLocalPlayer();

        // �ʱ�ȭ ��� �� ���� ����
        StartCoroutine(Reinitialize());
    }

    private void RemoveLocalPlayer()
    {
        if (photonView.IsMine)
        {
            Debug.Log("���� �÷��̾� ������Ʈ ���� ��...");
            PhotonNetwork.Destroy(photonView.gameObject);
        }
    }

    private IEnumerator Reinitialize()
    {
        // �ణ�� �����̸� �ξ� ������ Ȯ��
        yield return new WaitForSeconds(1f);

        Debug.Log("�� �÷��̾� ���� ��...");
        SpawnPlayer();

        // �ʱ�ȭ �Ϸ�
        isReinitializing = false;
        Debug.Log("�÷��̾� ���ʱ�ȭ �Ϸ�.");
    }
}
