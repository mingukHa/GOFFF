using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; //프리팹 배열
    [SerializeField] private Transform[] spawnPoints; //스폰 위치
    private bool hasSpawned = false; //스폰 확인용
    private void Start()
    {
        if (!PhotonNetwork.IsConnected) //연결 상태 보기
        {
            Debug.Log("포톤 서버와 연결이 안 되었음 로비로 이동");
            SceneManager.LoadScene("LoginScenes");
            return;
        }
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방에 입장하지 않았습니다. 방 입장을 기다립니다...");
            return;
        }
        StartCoroutine(WaitForRoomReady()); //방 생성 후 플레이어 생성
    }
    public override void OnJoinedRoom() //방에 입장 하면 호출
    {
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            SpawnPlayer();//생성이 안 되어 있으면 생성
        }
    }
    private IEnumerator WaitForRoomReady()
    {//네트워크 연결 및 방 입장 대기
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        SpawnPlayer();//입장이 완료 되면 프리팹을 생성
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null) //프리팹이 널
        {
            Debug.LogError("Player Prefab이 설정되지 않았습니다!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0) //스폰 포인트가 널
        {
            Debug.LogError("스폰 위치가 설정되지 않았습니다!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //배열번호 받고
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length]; //배열 번호에 맞는 위치에 스폰        

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);
        //플레이어를 포톤 네트워크로 생성 및 배열 번호에 맞는 위치로 프리팹,위치 설정
        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;
        }//스폰이 되었다면 트루로 설정해서 중복 생성 방지
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
       
    }     
    
}
