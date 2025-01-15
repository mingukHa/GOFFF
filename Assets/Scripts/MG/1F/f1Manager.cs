using Photon.Pun;
using UnityEngine;
using System.Collections;

public class f1playerspawn : MonoBehaviour
{
    public Transform[] TagObject; // 배열에 PlayerPos1, PlayerPos2 위치 설정
    private GameObject handOffset;
    private void Awake()
    {
        // PlayerPos1, PlayerPos2 위치 찾기
        GameObject playerPos1 = GameObject.Find("PlayerHolder(Clone)");
        GameObject playerPos2 = GameObject.Find("PlayerHolder1(Clone)");
        handOffset = GameObject.Find("handOffset") ?? transform.Find("handOffset")?.gameObject;


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
    private void Start()
    {
        StartCoroutine(FindAndToggleHandOffset());
    }
    private IEnumerator FindAndToggleHandOffset()
    {
        // handOffset GameObject를 찾아 대기 (최대 1초 대기)
        float timeout = 1f;
        float elapsedTime = 0f;

        while (handOffset == null && elapsedTime < timeout)
        {
            handOffset = GameObject.Find("handOffset");
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        if (handOffset == null)
        {
            Debug.LogError("handOffset GameObject를 찾을 수 없습니다.");
            yield break; // 코루틴 종료
        }

        Debug.Log("handOffset GameObject를 찾았습니다.");

        // handOffset GameObject 비활성화
        handOffset.SetActive(false);
        Debug.Log("handOffset 비활성화됨.");

        // 0.5초 대기
        yield return new WaitForSeconds(0.5f);

        // handOffset GameObject 활성화
        handOffset.SetActive(true);
        Debug.Log("handOffset 활성화됨.");
    }
}

