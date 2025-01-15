using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon 관련 라이브러리 추가

public class InDownElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // 할당된 엘리베이터 문 2개
    public float closeDuration = 2f; // 문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); // 닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // 열린 상태의 Scale

    private bool isClosing = false; // 문이 닫히는 중인지 확인
    private int isButtonOn = 0; // 버튼 상태를 로컬에서 관리
    public ElevatorDownTrigger elevatorTrigger;
    private bool runElevator = false;
    private Transform playerTr;
    public Transform ThreeFloorTp;


    private void Start()
    {
        // 델리게이트에 함수 연결
        elevatorTrigger.OnPlayerTriggered += HandlePlayersTriggered;
    }

    // 델리게이트에 등록할 함수
    private void HandlePlayersTriggered(Collider player)
    {
        Debug.Log("{player.name}이 엘리베이터에 들어왔습니다.");

        runElevator = true;
        playerTr = player.transform;
    }

    // SelectOnEnter 이벤트에 등록할 함수
    public void CloseDoors()
    {
        if (!runElevator && isClosing) return;
        isClosing = true;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDownDoors", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RPCDownDoors()
    {
        isClosing = true;
        StartCoroutine(CloseDoorsCoroutine());
    }


    // 문을 닫는 코루틴 함수
    public IEnumerator CloseDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor_SFX", gameObject);

        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   // 보간 비율 0 ~ 1 사이 값 계산
            foreach (var door in elevatorDoors)
            {
                door.localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;

            yield return null;  // 다음 프레임까지 대기
        }

        foreach (var door in elevatorDoors)
        {
            door.localScale = closedScale;
        }

        Debug.Log("문이 닫혔습니다.");
        isClosing = false; // 문 닫기 완료
        photonView.RPC("DownElevatorTp", RpcTarget.All); // 모든 클라이언트에 조건 확인 요청
    }

    [PunRPC]
    private void DownElevatorTp()
    {
        playerTr.position = ThreeFloorTp.position;
    }
}
