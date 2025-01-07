using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun; // Photon 관련 라이브러리 추가

public class InElevator : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> elevatorDoors; //할당된 엘리베이터 문 2개
    [SerializeField] private Transform elevatorBottom;    //엘리베이터 바닥
    public float closeDuration = 2f; //문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); //닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //열린 상태의 Scale

    public UpElevator upElevator;   //위로 버튼을 눌렀는지 확인하기 위해
    public DownElevator downElevator;   //아래로 버튼을 눌렀는지 확인하기 위해

    private bool isPlayerOnElevator = false; // 플레이어가 엘리베이터 바닥에 있는지 확인

    [PunRPC]
    public void CloseDoors()
    {
        photonView.RPC("CloseDoorsRPC", Photon.Pun.RpcTarget.All); // 모든 클라이언트에서 문을 닫도록 RPC 호출
    }

    [PunRPC]
    private void CloseDoorsRPC()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    public IEnumerator CloseDoorsCoroutine()
    {
        // 두 명 이상의 플레이어가 있을 경우 문 닫기, 텔레포트, 씬 이동 제한
        if (IsMultiplePlayersOnElevator())
        {
            Debug.Log("두 명 이상의 플레이어가 탑승 중입니다. 기능이 제한됩니다.");
            yield break; // 함수 종료, 문 닫기 및 이후 코드 실행 안 함
        }

        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   //보간 비율 0 ~ 1 사이 값 계산
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //다음 프레임까지 대기
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }

        if (isPlayerOnElevator)
        {
            CheckElevatorConditions();
        }
    }

    private void CheckElevatorConditions()
    {
        // 두 명 이상의 플레이어가 있을 경우 씬 이동, 텔레포트 방지
        if (IsMultiplePlayersOnElevator())
        {
            Debug.Log("두 명 이상의 플레이어가 탑승 중입니다. 기능이 제한됩니다.");
            return; // 씬 이동 및 텔레포트 진행하지 않음
        }

        // UpElevator의 isUpDoorOpening이 true일 때 다음 씬으로 이동
        if (upElevator != null && upElevator.isUpDoorOpening)
        {
            LoadNextScene();
        }
        // DownElevator의 isDownDoorOpening이 true일 때 텔레포트
        else if (downElevator != null && downElevator.isDownDoorOpening)
        {
            TeleportPlayerToOrigin();
        }
        else
        {
            Debug.Log("엘리베이터 상태가 유효하지 않습니다.");
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 현재 씬의 빌드 인덱스 가져오기
        int nextSceneIndex = currentSceneIndex + 1;                       // 다음 씬 인덱스 계산

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)      // 빌드 세팅 안에 씬이 존재하는지 확인
        {
            SceneManager.LoadScene(nextSceneIndex);                       // 다음 씬 로드
        }
        else
        {
            Debug.Log("마지막 씬입니다. 더 이상 씬이 없습니다.");
        }
    }

    private void TeleportPlayerToOrigin()
    {
        GameObject player = PhotonNetwork.LocalPlayer.TagObject as GameObject; // Photon에서 현재 로컬 플레이어 찾기

        if (player != null)
        {
            player.transform.position = new Vector3(0, 0, 0); // 월드 좌표계 기준으로 (0, 0, 0) 위치로 텔레포트
            Debug.Log("플레이어를 (0, 0, 0) 위치로 텔레포트 시켰습니다.");
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
        }
    }

    private bool IsMultiplePlayersOnElevator()
    {
        // Photon 네트워크에서 엘리베이터 바닥에 있는 플레이어 수 체크
        int playerCount = 0;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = player.TagObject as GameObject;
            if (playerObject != null && playerObject.GetComponent<Collider>().
                bounds.Intersects(elevatorBottom.GetComponent<Collider>().bounds))
            {
                playerCount++;
            }
        }

        return playerCount > 1; // 두 명 이상의 플레이어가 있으면 true
    }

    // 엘리베이터 바닥과 충돌 감지 (지속적으로 플레이어 상태 확인)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            Debug.Log("플레이어가 엘리베이터 바닥에 탑승 중입니다.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            Debug.Log("플레이어가 엘리베이터 바닥에서 내렸습니다.");
        }
    }
}