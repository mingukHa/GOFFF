using Photon.Pun;
using UnityEngine;

public class ObjectPrefabSpawner : MonoBehaviourPun
{
    [SerializeField] private string can; // ������ Photon ������ �̸�
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(30, 0, 30); // ���� ���� ũ��
    [SerializeField] private Vector3 spawnAreaCenter = Vector3.zero; // ���� ���� �߽�
    [SerializeField] private int numberOfPrefabs = 5; // ������ ������ ����

    void Start()
    {
        SpawnMultiplePrefabs(numberOfPrefabs);
    }

    public void SpawnMultiplePrefabs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnRandomPrefab();
        }
    }

    public void SpawnRandomPrefab()
    {
        // ���� ��ġ ���
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            spawnAreaCenter.y,
            Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
        );

        // ���� ȸ�� ����
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // Photon ������ ����
        PhotonNetwork.Instantiate(can, randomPosition, randomRotation);

        Debug.Log($"Prefab '{can}' spawned at {randomPosition} with rotation {randomRotation.eulerAngles}");
    }
}
