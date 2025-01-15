using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon 관련 라이브러리 추가

public class InElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // 할당된 엘리베이터 문 2개
    public float closeDuration = 2f; // 문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); // 닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // 열린 상태의 Scale

    private bool isClosing = false; // 문이 닫히는 중인지 확인
    public ElevatorTrigger elevatorTrigger;

    private bool runElevator = false;

    private void Start()
    {
        // 델리게이트에 함수 연결
        elevatorTrigger.OnPlayersTriggered += HandlePlayersTriggered;
    }

    private void HandlePlayersTriggered(Collider player1, Collider player2)
    {
        Debug.Log($"Player1: {player1.name}, Player2: {player2.name}");

        runElevator = true;
    }

    //[PunRPC]
    public void CloseDoors()
    {
        if (!runElevator && isClosing) return;
        isClosing = true;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDoorsCoroutine", RpcTarget.Others);
        }
    }


    [PunRPC]
    private void RPCDoorsCoroutine()
    {
        isClosing = true;
        StartCoroutine(CloseDoorsCoroutine());
    }


    public IEnumerator CloseDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor_SFX",gameObject);

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
        photonView.RPC("CheckElevatorConditions", RpcTarget.All); // 모든 클라이언트에 조건 확인 요청
    }

    [PunRPC]
    public void CheckElevatorConditions()
    {

        if(PhotonNetwork.IsMasterClient) 
        {

            Debug.Log("다음 씬으로 이동합니다.");
            PhotonNetwork.LoadLevel("JHScenes3");

        }
    }

}
