using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun; // Photon 관련 라이브러리 추가

public class InElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //할당된 엘리베이터 문 2개
    [SerializeField] private Transform elevatorBottom;    //엘리베이터 바닥
    public float closeDuration = 2f; //문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); //닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //열린 상태의 Scale

    public UpElevator upElevator;   //위로 버튼을 눌렀는지 확인하기 위해
    public DownElevator downElevator;   //아래로 버튼을 눌렀는지 확인하기 위해

    [PunRPC]
    public void CloseDoorsRPC()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    public IEnumerator CloseDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor2_SFX");

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

        CheckElevatorConditions();
    }

    private void CheckElevatorConditions()
    {
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
}