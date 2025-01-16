using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
public class NewGameOver : MonoBehaviourPun
{
    private GameObject Player1; // 로컬 플레이어
    private GameObject Player2; // 상대 플레이어
    private GameObject monster;

    [SerializeField] private Transform spwan1;       // 플레이어1 스폰 지점
    [SerializeField] private Transform spwan2;       // 플레이어2 스폰 지점
    [SerializeField] private Transform monsterspawn; // 몬스터 스폰 지점

    private void Start()
    {
        StartCoroutine(WaitForPlayersAndMonster()); // 플레이어와 몬스터 참조 확인
    }

    private IEnumerator WaitForPlayersAndMonster()
    {
        while (Player1 == null || Player2 == null || monster == null)
        {
            Debug.Log("플레이어 및 몬스터 참조 재시도 중...");
            FindPlayersAndMonster();
            yield return new WaitForSeconds(0.5f); // 0.5초마다 재시도
        }
        Debug.Log("플레이어 및 몬스터 참조 완료.");
    }

    private void FindPlayersAndMonster()
    {
        foreach (PhotonView view in PhotonNetwork.PhotonViews)
        {
            if (view != null && view.Owner != null)
            {
                if (view.Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    Player1 = view.gameObject; // 로컬 플레이어
                    Debug.Log("Player1 할당 완료.");
                }
                else if (view.Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    Player2 = view.gameObject; // 상대 플레이어
                    Debug.Log("Player2 할당 완료.");
                }
            }
        }

        // 몬스터는 태그로 찾기
        monster = GameObject.FindWithTag("Monster");
        if (monster != null)
        {
            Debug.Log("Monster 할당 완료.");
        }
        else
        {
            Debug.LogError("Monster를 찾을 수 없습니다.");
        }
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ReStart 호출: 위치를 고정 스폰 지점으로 갱신 중...");
            photonView.RPC("UpdatePlayerPosition", RpcTarget.All, spwan1.position, spwan2.position, monsterspawn.position);
        }
    }

    [PunRPC]
    private void UpdatePlayerPosition(Vector3 position1, Vector3 position2, Vector3 monsterPosition)
    {
        // Player1 위치 변경
        if (Player1 != null)
        {
            UpdatePosition(Player1, position1);
            Debug.Log($"Player1 위치 변경 완료: {position1}");
        }

        // Player2 위치 변경
        if (Player2 != null)
        {
            UpdatePosition(Player2, position2);
            Debug.Log($"Player2 위치 변경 완료: {position2}");
        }

        // Monster 위치 변경
        if (monster != null)
        {
            UpdatePosition(monster, monsterPosition, true); // 몬스터는 NavMeshAgent 처리
            Debug.Log($"Monster 위치 변경 완료: {monsterPosition}");
        }
    }

    private void UpdatePosition(GameObject obj, Vector3 newPosition, bool isMonster = false)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // 물리 계산 비활성화
            obj.transform.position = newPosition; // 위치 변경
            rb.isKinematic = false; // 물리 계산 다시 활성화
        }
        else
        {
            obj.transform.position = newPosition; // Rigidbody가 없을 경우
        }

        // 몬스터의 경우 NavMeshAgent 처리
        if (isMonster)
        {
            var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(newPosition);
            }
        }
    }
}
