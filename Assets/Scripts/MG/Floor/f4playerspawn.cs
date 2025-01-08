using Photon.Pun;
using UnityEngine;

public class f4playerspawn : MonoBehaviour
{
    public Transform[] TagObject;
    private void Awake()
    {
        // PlayerPos1, PlayerPos2 위치 찾기
        GameObject playerPos1 = GameObject.Find("PlayerPos1");
        GameObject playerPos2 = GameObject.Find("PlayerPos2");
        if (playerPos1 == null || playerPos2 == null)
        {
            Debug.LogError("PlayerPos1 또는 PlayerPos2가 씬에 없습니다!");
            return;
        }
        // 로컬 플레이어의 ActorNumber를 기준으로 위치 설정
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerIndex == 1)
        {
            // 첫 번째 플레이어는 PlayerPos1 위치로 이동
            TagObject[playerIndex].position = playerPos1.transform.position;
            Debug.Log("플레이어 1이 PlayerPos1으로 이동되었습니다.");
        }
        else if (playerIndex == 2)
        {
            // 두 번째 플레이어는 PlayerPos2 위치로 이동
            TagObject[playerIndex].position = playerPos2.transform.position;
            Debug.Log("플레이어 2가 PlayerPos2로 이동되었습니다.");
        }
        else
        {
            Debug.LogWarning("지원되지 않는 플레이어 인덱스입니다.");
        }
    }
}
