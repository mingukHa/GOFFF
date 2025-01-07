using UnityEngine;
using Photon.Pun;

public class PlayerReinitializer : MonoBehaviourPun
{
    [SerializeField] private GameObject playerPrefab; // �÷��̾� ������
    [SerializeField] private Transform[] spawnPoints; // ���� ��ġ �迭
    private bool isReinitializing = false;

    public void ReinitializePlayer()
    {
        if (isReinitializing) return; // �̹� ���ʱ�ȭ ���̸� �ߺ� ���� ����

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

    private System.Collections.IEnumerator Reinitialize()
    {
        // �ణ�� �����̸� �ξ� �ʱ�ȭ ������ Ȯ��
        yield return new WaitForSeconds(1f);

        Debug.Log("�� �÷��̾� ���� ��...");
        SpawnPlayer();

        // �ʱ�ȭ �Ϸ� �÷��� ����
        isReinitializing = false;
        Debug.Log("�÷��̾� ���ʱ�ȭ �Ϸ�.");
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
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� ������Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
    }
}
