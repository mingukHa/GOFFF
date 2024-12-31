using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹
    [SerializeField] private Transform[] spawnPoints; // 스폰 위치 배열
    private bool hasSpawned = false; // 플레이어가 스폰되었는지 확인

    private void Start()
    {
        // 스폰 포인트 확인
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("스폰 포인트가 설정되지 않았습니다! 기본 위치를 사용합니다.");
            spawnPoints = new Transform[] { new GameObject("DefaultSpawn").transform };
            spawnPoints[0].position = Vector3.zero;
        }

        // Photon 서버 연결 상태 확인
        if (!PhotonNetwork.IsConnected)
        {
            Debug.LogWarning("Photon 서버에 연결되지 않았습니다. 로비로 복귀합니다.");
            SceneManager.LoadScene("LobbyScene");
            return;
        }

        // 방 입장 상태 확인
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방에 입장하지 않았습니다. 방 입장을 기다립니다...");
            return;
        }

        // 방에 이미 입장한 경우 즉시 플레이어 스폰
        StartCoroutine(WaitForRoomReady());
    }

    public override void OnJoinedRoom()
    {
        // 방 입장이 완료되었을 때 호출
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            SpawnPlayer();
        }
    }

    private IEnumerator WaitForRoomReady()
    {
        // Photon 서버와 방 상태가 준비될 때까지 대기
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        SpawnPlayer();
    }

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

        // ActorNumber를 기반으로 스폰 위치 결정
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        if (spawnPoint == null)
        {
            Debug.LogWarning($"스폰 포인트가 null입니다! Index: {playerIndex}. 기본 위치를 사용합니다.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // 네트워크 상에 플레이어 프리팹 생성
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true; // 스폰 완료 플래그 설정
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }

        // 충돌 방지 처리 (Collider 비활성화 후 활성화)
        StartCoroutine(ReenableCollider(player));
    }

    private IEnumerator ReenableCollider(GameObject player)
    {
        if (player == null) yield break;

        Collider collider = player.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // 충돌 비활성화
            yield return new WaitForSeconds(1); // 1초 후 다시 활성화
            collider.enabled = true;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");

        // 방 상태 UI 업데이트
        UpdatePlayerListUI();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log($"플레이어 {otherPlayer.NickName}이(가) 방에서 나갔습니다.");

        // 방 상태 UI 업데이트
        UpdatePlayerListUI();
    }

    private void UpdatePlayerListUI()
    {
        Debug.Log("현재 방의 플레이어 목록:");
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"플레이어: {player.NickName}");
        }
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarning($"Photon 서버 연결 끊김: {cause}. 로비로 복귀합니다.");
        SceneManager.LoadScene("LobbyScene");
    }
}
