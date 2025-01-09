using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Waitscene gameManager; // 게임 매니저 참조

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어인지 확인
        {
            gameManager.UpdateReadyCount(1); // 게임 매니저에 플레이어 준비 상태 전달
            Debug.Log("발판 활성화");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어인지 확인
        {
            gameManager.UpdateReadyCount(-1); // 게임 매니저에 플레이어 준비 상태 전달
            Debug.Log("발판 빠짐");
        }
    }
}
