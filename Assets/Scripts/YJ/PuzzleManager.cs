using Photon.Pun;
using UnityEngine;

public class PuzzleManager : MonoBehaviourPunCallbacks
{
    public SocketValidator[] socketValidators; // 도플갱어 방에 있는 모든 SocketValidator
    public GameObject jailBar; // 감옥 쇠창살 오브젝트
    [SerializeField]
    private GameObject Monster; // 몬스터 Prefab (Resources 폴더에 있어야 함)
    [SerializeField]
    private GameObject spawnpoint; // 몬스터가 생성될 위치

    public void CheckPuzzleStatus()
    {
        if (socketValidators == null || socketValidators.Length == 0)
        {
            Debug.LogError("SocketValidators array is empty or not assigned!");
            return;
        }

        foreach (var validator in socketValidators)
        {
            if (!validator.IsObjectCorrectlyPlaced())
            {
                Debug.Log("Puzzle not solved yet.");
                return;
            }
        }
        Debug.Log("Puzzle solved!");
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        if (jailBar != null)
        {
            jailBar.SetActive(false);
            Debug.Log("Jailbar disabled!");
        }
        else
        {
            Debug.LogError("Jailbar not found!");
        }

        // 몬스터 스폰 호출
        MonsterSpawn();
    }

    private void MonsterSpawn()
    {
        if (Monster == null || spawnpoint == null)
        {
            Debug.LogError("Monster or spawnpoint is not assigned!");
            return;
        }

        // PhotonNetwork.Instantiate를 통해 몬스터 스폰
        PhotonNetwork.Instantiate(Monster.name, spawnpoint.transform.position, spawnpoint.transform.rotation);
    }
}
