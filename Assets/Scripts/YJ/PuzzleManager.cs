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
            Debug.LogError("SocketValidators 배열이 Null이거나 지정되지 않았습니다.");
            return;
        }

        foreach (var validator in socketValidators)
        {
            if (!validator.IsObjectCorrectlyPlaced())
            {
                Debug.Log("퍼즐이 아직 풀리지 않았습니다.");
                return;
            }
        }
        Debug.Log("퍼즐이 풀렸습니다!");
        photonView.RPC("SolvePuzzle", RpcTarget.All);
    }

    [PunRPC]
    private void SolvePuzzle()
    {
        if (jailBar != null)
        {
            jailBar.SetActive(false);
            Debug.Log("쇠창살 비활성화!");
        }
        else
        {
            Debug.LogError("쇠창살 오브젝트를 찾을 수 없습니다!");
        }

        // 몬스터 스폰 호출
        MonsterSpawn();
    }

    private void MonsterSpawn()
    {
        if (Monster == null || spawnpoint == null)
        {
            Debug.LogError("몬스터 스폰 위치가 지정되지 않았습니다!");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.Instantiate를 통해 몬스터 스폰
            PhotonNetwork.Instantiate(Monster.name, spawnpoint.transform.position, spawnpoint.transform.rotation);
        }
    }
}
