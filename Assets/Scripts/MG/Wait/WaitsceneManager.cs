using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; //플레이어 프리팹을 받아 올 배열
    [SerializeField] private Transform[] spawnPoints; //플레이어의 스폰 위치
    [SerializeField] private GameObject button1; //엘리베이터 버튼1
    [SerializeField] private GameObject button2; //엘리베이터 버튼 2
    public string Scene = "MainScenes"; //로드 할 씬의 이름을 인스펙터로 수정 가능
    private bool hasSpawned = false; //스폰 유무
    private int readyPlayerCount = 0; // 준비 완료된 플레이어 수

    private void Start()
    {
        if (!PhotonNetwork.IsConnected) //포톤 네트워크와 연결이 안 되어 있다면
        {
            Debug.Log("포톤 서버와 연결이 안 되었음 로비로 이동");
            SceneManager.LoadScene("LoginScenes"); //로그인 씬으로 다시 이동
            return;
        }
        if (!PhotonNetwork.InRoom) //방에 입장을 못 했다면
        {
            Debug.Log("현재 방에 입장하지 않았습니다. 방 입장을 기다립니다...");
            return; 
        }
        StartCoroutine(WaitForRoomReady()); //방 입장 대기 코루틴 실행
    }

    public override void OnJoinedRoom() //방에 들어오고
    {
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned) //스폰이 안 되었다면
        {
            SpawnPlayer(); //스폰
        }
    }

    private IEnumerator WaitForRoomReady() //입장 및 생성 대기 코루틴
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom); //연결,방에 들어왔다면
        SpawnPlayer(); //플레이어 스폰
    }

    private void SpawnPlayer() //스폰 함수
    {
        if (playerPrefab == null) //프리팹이 없으면
        {
            Debug.LogError("Player Prefab이 설정되지 않았습니다!");
            return; //돌아감
        }

        if (spawnPoints == null || spawnPoints.Length == 0) //스폰 배열이 없다면 돌아감
        {
            Debug.LogError("스폰 위치가 설정되지 않았습니다!");
            return; 
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //액트넘버는 1부터 시작, 배열은 0부터 시작
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length]; //액트넘버와 맞는 배열위치로 스폰

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);
        //플레이어 액트 넘버에 맞는 플레이어 프리팹 생성
        
        hasSpawned = true;
        //생성 후 트루

        StartCoroutine(ReenableCollider(player));

        // 모든 클라이언트에 DontDestroyOnLoad 적용
        photonView.RPC("SetDontDestroyOnLoad", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }
    //씬이 넘어가며 컨트롤러가 자꾸 동기화 오류가 심하게 생김. 그러므로 돈 디스트로이로 관리
    [PunRPC]
    private void SetDontDestroyOnLoad(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.gameObject != null)
        {
            DontDestroyOnLoad(targetView.gameObject);
            Debug.Log("모든 PlayerHolder에 DontDestroyOnLoad 적용 완료.");
        }
    }
    //생성하고 콜라이더가 부딛히면 저 멀리 날아감. 1초의 대기시간 주기
    private IEnumerator ReenableCollider(GameObject player)
    {
        if (player == null) yield break;

        Collider collider = player.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(1);
            collider.enabled = true;
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    public void OnButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient) //방장이라면
        {
            photonView.RPC("PlayerReady", RpcTarget.AllBuffered); // 모든 클라이언트에 플레이어 준비 상태 전달
            Debug.Log($"현재 준비된 플레이어 수: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");
            
            // 모든 플레이어가 준비되었을 경우 다음 씬으로 전환
            if (readyPlayerCount == 2)
            {

                Debug.Log("2명 준비 완료! 다음 씬으로 이동합니다.");
                PhotonNetwork.LoadLevel(Scene); // 전환할 씬 이름으로 변경

            }
            else
            {
                return;
            }
        }
        Debug.Log("버튼이 눌렸습니다");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]
    public void PlayerReady()
    {
        readyPlayerCount++; //레디 카운트 1 증가
    }
}
