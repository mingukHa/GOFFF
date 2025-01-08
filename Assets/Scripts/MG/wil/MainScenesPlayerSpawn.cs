using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; // 프리팹 배열
    [SerializeField] private Transform[] spawnPoints;   // 스폰 위치
    private bool hasSpawned = false;                   // 스폰 확인용

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
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

        Debug.Log("현재 방에 입장했습니다. 다른 플레이어를 기다립니다...");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다. 현재 플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("모든 플레이어가 입장했습니다. 플레이어를 스폰합니다.");
            SpawnPlayer();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}. 현재 플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !hasSpawned)
        {
            Debug.Log("모든 플레이어가 입장했습니다. 플레이어를 스폰합니다.");
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || playerPrefab.Length == 0)
        {
            Debug.LogError("Player Prefab이 설정되지 않았습니다!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 위치가 설정되지 않았습니다!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // 프리팹 및 스폰 포인트 인덱스 계산
        GameObject selectedPrefab = playerPrefab[playerIndex % playerPrefab.Length];
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        GameObject player = PhotonNetwork.Instantiate(selectedPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }
}
