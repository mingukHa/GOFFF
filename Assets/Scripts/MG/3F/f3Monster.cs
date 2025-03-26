using Photon.Pun;
using UnityEngine;

public class F3Monster : MonoBehaviourPun //3�� ���� ����
{
    [SerializeField] private GameObject monsterPrefab; // ��ȯ�� ���� ������
    [SerializeField] private Transform spawnPoints;  // ���Ͱ� ��ȯ�� ���� ����Ʈ �迭

    public void SpawnMonster()
    {

        Transform spawnPoint = spawnPoints;

        PhotonNetwork.Instantiate(monsterPrefab.name, spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"���Ͱ� {spawnPoint.position}���� ��ȯ�Ǿ����ϴ�.");
    }
}
