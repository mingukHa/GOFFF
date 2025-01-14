using System;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    // 델리게이트 선언
    public Action<Collider, Collider> OnPlayersTriggered;

    private Collider firstPlayer;
    private Collider secondPlayer;

    private void OnTriggerEnter(Collider other)
    {
        // "Player" 태그인지 확인
        if (other.CompareTag("Player"))
        {
            if (firstPlayer == null)
            {
                // 첫 번째 플레이어 등록
                firstPlayer = other;
            }
            else if (secondPlayer == null && other.name != firstPlayer.name)
            {
                // 두 번째 플레이어 등록
                secondPlayer = other;

                // 두 플레이어가 모두 트리거 안에 있을 때 델리게이트 실행
                OnPlayersTriggered?.Invoke(firstPlayer, secondPlayer);

                // 초기화 (재사용 가능)
                ResetPlayers();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거를 나가면 초기화
        if (other == firstPlayer)
        {
            firstPlayer = null;
        }
        else if (other == secondPlayer)
        {
            secondPlayer = null;
        }
    }

    // 초기화 함수
    private void ResetPlayers()
    {
        firstPlayer = null;
        secondPlayer = null;
    }
}
