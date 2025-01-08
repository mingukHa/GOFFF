using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private bool hasSpawned = false; // 중복 생성 방지

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon 서버와 연결되지 않았습니다. 로비로 이동합니다.");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방에 입장하지 않았습니다.");
            return;
        }

        Debug.Log("마스터 클라이언트가 인원을 확인하여 플레이어를 생성합니다.");
        CheckAndSpawnPlayers();
    }

    private void CheckAndSpawnPlayers()
    {
        if (PhotonNetwork.IsMasterClient) // 현재 클라이언트가 마스터 클라이언트인지 확인
        {
            Debug.Log("마스터 클라이언트가 인원을 확인 중...");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) // 방 인원이 2명일 경우
            {
                Debug.Log("방 인원이 2명입니다. 플레이어를 생성합니다.");
                SpawnPlayersForAll();
            }
            else
            {
                Debug.Log("인원이 부족합니다. 기다립니다...");
            }
        }
    }

    private void SpawnPlayersForAll()
    {
        if (hasSpawned) return; // 중복 생성 방지

        // 모든 클라이언트에서 플레이어를 생성하도록 RPC 호출
        photonView.RPC("SpawnPlayer", RpcTarget.All);
        hasSpawned = true;
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab이 설정되지 않았습니다!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 위치가 설정되지 않았습니다!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; // ActorNumber 기반으로 스폰 위치 결정
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 생성되었습니다.");
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");

        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트만 인원 확인
        {
            CheckAndSpawnPlayers();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"플레이어 {otherPlayer.NickName}이(가) 방에서 나갔습니다.");
    }
}
