using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class F3Spawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private bool hasSpawned = false;

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

        StartCoroutine(WaitForRoomReady());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            StartCoroutine(SpawnPlayerWithDelay());
        }
    }

    private IEnumerator WaitForRoomReady()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        // ActorNumber를 기반으로 생성 지연 시간 계산
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        float spawnDelay = playerIndex * 1.0f; // 각 플레이어마다 1초 지연

        Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) {spawnDelay}초 후 스폰됩니다.");
        yield return new WaitForSeconds(spawnDelay);

        // RPC 호출: 다른 클라이언트와 동기화
        photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
    }
    [PunRPC]
    private void SpawnPlayer(int actorNumber)
    {
        if (hasSpawned) return; // 이미 생성된 경우 실행 방지
        hasSpawned = true;

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

        int playerIndex = actorNumber - 1; // ActorNumber 기반으로 스폰 위치 결정
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        if (spawnPoint == null)
        {
            Debug.LogWarning($"스폰 포인트가 null입니다! Index: {playerIndex}. 기본 위치를 사용합니다.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }

    

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");
    }
}
