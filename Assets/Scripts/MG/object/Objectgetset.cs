using Photon.Pun;
using UnityEngine;

public class ObjectPrefabSpawner : MonoBehaviourPun
{
    [SerializeField] private string can; // 생성할 Photon 프리팹 이름
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(30, 0, 30); // 생성 영역 크기
    [SerializeField] private Vector3 spawnAreaCenter = Vector3.zero; // 생성 영역 중심
    [SerializeField] private int numberOfPrefabs = 5; // 생성할 프리팹 개수

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
        // 랜덤 위치 계산
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            spawnAreaCenter.y,
            Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
        );

        // 랜덤 회전 설정
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // Photon 프리팹 생성
        PhotonNetwork.Instantiate(can, randomPosition, randomRotation);

        Debug.Log($"Prefab '{can}' spawned at {randomPosition} with rotation {randomRotation.eulerAngles}");
    }
}
