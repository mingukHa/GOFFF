using Photon.Pun;
using UnityEngine;

public class f1layerspawn : MonoBehaviour
{
    public Transform[] TagObject; // 배열에 PlayerPos1, PlayerPos2 위치 설정

    private void Awake()
    {
        // PlayerPos1, PlayerPos2 위치 찾기
        GameObject playerPos1 = GameObject.Find("PlayerHolder(Clone)");
        GameObject playerPos2 = GameObject.Find("PlayerHolder1(Clone)");

        if (playerPos1 == null)
        {
            Debug.LogError("PlayerPos1이 씬에 없습니다!");
            return;
        }
        if (playerPos2 == null)
        {
            Debug.LogError("PlayerPos2가 씬에 없습니다!");
            return;
        }

        // 로컬 플레이어의 ActorNumber를 기준으로 위치 설정
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

        if (playerIndex < 1 || playerIndex > TagObject.Length)
        {
            Debug.LogError($"잘못된 플레이어 인덱스: {playerIndex}. TagObject 배열의 크기를 확인하세요.");
            return;
        }

        if (playerIndex == 1)
        {
            // 첫 번째 플레이어는 PlayerPos1 위치로 이동
            playerPos1.transform.position = TagObject[playerIndex - 1].position; // 인덱스 보정
            Debug.Log("플레이어 1이 PlayerPos1으로 이동되었습니다.");
        }
        else if (playerIndex == 2)
        {
            // 두 번째 플레이어는 PlayerPos2 위치로 이동
            playerPos2.transform.position = TagObject[playerIndex - 1].position; // 인덱스 보정
            Debug.Log("플레이어 2가 PlayerPos2로 이동되었습니다.");
        }
        else
        {
            Debug.LogWarning("지원되지 않는 플레이어 인덱스입니다.");
        }
    }
}
