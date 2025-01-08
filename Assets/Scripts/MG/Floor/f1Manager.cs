using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class f1Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; // 프리팹 배열
    [SerializeField] private Transform[] spawnPoints;   // 스폰 위치
    private bool hasSpawned = false;                   // 스폰 확인용

    private void Start()
    {
        // 포톤 네트워크 연결 확인
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("포톤 서버와 연결이 안 되었음 로비로 이동");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        // 방 입장 여부 확인
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방에 입장하지 않았습니다. 방 입장을 기다립니다...");
            return;
        }

        // 입장이 완료되면 플레이어 입장 대기
        Debug.Log("현재 방에 입장했습니다. 다른 플레이어를 기다립니다...");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // 다른 플레이어가 방에 입장했을 때 호출
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다. 현재 플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 방에 플레이어가 2명이 되면 스폰
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("모든 플레이어가 입장했습니다. 플레이어를 스폰합니다.");
            SpawnPlayer();
        }
    }

   public override void OnJoinedRoom()
   {
       // 자신이 방에 입장했을 때 호출
       Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}. 현재 플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

       // 이미 방에 2명이 있다면 스폰 (다른 클라이언트가 먼저 들어와 있는 경우)
       if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
       {
           Debug.Log("모든 플레이어가 입장했습니다. 플레이어를 스폰합니다.");
           SpawnPlayer();
       }
   }

    private void SpawnPlayer()
    {
        // 플레이어 프리팹 배열이 설정되지 않은 경우
        if (playerPrefab == null || playerPrefab.Length == 0)
        {
            Debug.LogError("Player Prefab이 설정되지 않았습니다!");
            return;
        }

        // 스폰 포인트 배열이 설정되지 않은 경우
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 위치가 설정되지 않았습니다!");
            return;
        }

        // 플레이어의 고유 ActorNumber를 기반으로 스폰 위치 계산
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        // 네트워크 상에서 플레이어 생성
        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true; // 중복 생성 방지
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }
}
