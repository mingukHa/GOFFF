using System;
using UnityEngine;

public class ElevatorDownTrigger : MonoBehaviour
{
    // 델리게이트 선언
    public Action<Collider> OnPlayerTriggered;

    private Collider Player;

    private void OnTriggerEnter(Collider other)
    {
        // "Player" 태그인지 확인
        if (other.CompareTag("Player"))
        {
            if (Player == null)
            {
                // 첫 번째 플레이어 등록
                Player = other;

                // 두 플레이어가 모두 트리거 안에 있을 때 델리게이트 실행
                OnPlayerTriggered?.Invoke(Player);

                // 초기화 (재사용 가능)
                ResetPlayers();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거를 나가면 초기화
        if (other == Player)
        {
            Player = null;
        }
    }

    // 초기화 함수
    private void ResetPlayers()
    {
        Player = null;
    }
}
