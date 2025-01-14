using Photon.Pun;
using UnityEngine;

public class PuzzleManager : MonoBehaviourPunCallbacks
{
    public SocketValidator[] socketValidators; // 도플갱어 방에 있는 모든 SocketValidator
    public BoxCollider eVButton; // 엘리베이터 버튼
    [SerializeField]
    private GameObject Monster; // 몬스터 Prefab (Resources 폴더에 있어야 함)


    private void Start()
    {
        eVButton.enabled = false;
    }

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
        if (eVButton != null)
        {
            eVButton.enabled = true;
            Debug.Log("엘리베이터 버튼 활성화!");
        }
        else
        {
            Debug.LogError("엘리베이터 버튼을 찾을 수 없습니다!");
        }

        // 몬스터 스폰 호출
        Debug.Log("좀비가 생성 됨");
        MonsterSpawn();
        photonView.RPC("MonsterSpawn", RpcTarget.All);
    }
    [PunRPC]
    private void MonsterSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Monster.SetActive(true);
        }
    }
}
