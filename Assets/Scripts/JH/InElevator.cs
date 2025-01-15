using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon 관련 라이브러리 추가

public class InElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // 할당된 엘리베이터 문 2개
    //[SerializeField] private Transform elevatorBottom;    // 엘리베이터 바닥
    public float closeDuration = 2f; // 문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); // 닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // 열린 상태의 Scale

    //public UpElevator upElevator;   // 위로 버튼을 눌렀는지 확인하기 위해
    //public DownElevator downElevator;   // 아래로 버튼을 눌렀는지 확인하기 위해

    private bool isClosing = false; // 문이 닫히는 중인지 확인
    private int isButtonOn = 0; // 버튼 상태를 로컬에서 관리
    public ElevatorTrigger elevatorTrigger;

    private bool runElevator = false;


    //private void OnTriggerEnter(Collider other)
    //{
    //    // Collider로 감지된 오브젝트가 플레이어인지 확인
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        GameObject player = other.gameObject;

    //        if (player == PhotonNetwork.LocalPlayer.TagObject as GameObject)
    //        {
    //            Debug.Log("로컬 플레이어가 엘리베이터에 들어왔습니다.");
    //            photonView.RPC("CheckElevatorConditions", RpcTarget.All);
    //        }
    //    }
    //}


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
        //isButtonOn++;
        //photonView.RPC("RPCIsButtonOn", RpcTarget.Others);
        if (!runElevator && isClosing) return;
        isClosing = true;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDoorsCoroutine", RpcTarget.Others);
        }
    }
    //[PunRPC]
    //public void RPCIsButtonOn()
    //{
    //    isButtonOn++;
    //}

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
        // UpElevator의 isUpDoorOpening이 true일 때 다음 씬으로 이동
        //if (upElevator != null && upElevator.isUpDoorOpening)
        if(PhotonNetwork.IsMasterClient) 
        {
            //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //int nextSceneIndex = currentSceneIndex + 1;
            //Debug.Log($"{currentSceneIndex}인덱스 씬{nextSceneIndex}다음씬");

            //if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            //{
                Debug.Log("다음 씬으로 이동합니다.");
                PhotonNetwork.LoadLevel("JHScenes3");
            //}
            //else
            //{
            //    Debug.Log("마지막 씬입니다. 더 이상 씬이 없습니다.");
            //}
        }
        //else
        //{
        //    Debug.Log("엘리베이터 상태가 유효하지 않습니다.");
        //}
    }

    //[PunRPC]
    //public void LoadNextScene()
    //{
    //    //if (isButtonOn >= 2) return; // 씬이 이미 로드되었는지 확인

    //    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    int nextSceneIndex = currentSceneIndex + 1;
    //    Debug.Log($"{currentSceneIndex}인덱스 씬{nextSceneIndex}다음씬");
        
    //    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    //    {
    //        Debug.Log("다음 씬으로 이동합니다.");           
    //        PhotonNetwork.LoadLevel(nextSceneIndex);
    //    }
    //    else
    //    {
    //        Debug.Log("마지막 씬입니다. 더 이상 씬이 없습니다.");
    //    }
    //}
}
