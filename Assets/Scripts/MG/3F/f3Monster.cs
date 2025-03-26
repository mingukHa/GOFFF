using Photon.Pun;
using UnityEngine;

public class F3Monster : MonoBehaviourPun //3층 몬스터 스폰
{
    [SerializeField] private GameObject monsterPrefab; // 소환할 몬스터 프리팹
    [SerializeField] private Transform spawnPoints;  // 몬스터가 소환될 스폰 포인트 배열

    public void SpawnMonster()
    {

        Transform spawnPoint = spawnPoints;

        PhotonNetwork.Instantiate(monsterPrefab.name, spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"몬스터가 {spawnPoint.position}에서 소환되었습니다.");
    }
}
