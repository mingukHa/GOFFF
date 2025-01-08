using Photon.Pun;
using UnityEngine;

public class f3Monster : MonoBehaviourPun
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
