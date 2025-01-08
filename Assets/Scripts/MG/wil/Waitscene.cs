using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;

    private bool hasSpawned = false;
    private int readyPlayerCount = 0;

    private GameObject playerInstance; // 플레이어 인스턴스 저장

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

        // 씬 로드 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator WaitForRoomReady()
    {
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

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        playerInstance = PhotonNetwork.Instantiate(playerPrefab[playerIndex % playerPrefab.Length].name, spawnPoint.position, spawnPoint.rotation);

        if (playerInstance != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;

            DontDestroyOnLoad(playerInstance);
            Debug.Log($"{playerInstance.name} 파괴방지 설정 완료");
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드 완료: {scene.name}. 플레이어 동기화 시작.");

        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", RpcTarget.OthersBuffered, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }

    [PunRPC]
    private void SyncPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerInstance == null)
        {
            // 로컬에서 플레이어를 재생성
            playerInstance = PhotonNetwork.Instantiate(playerPrefab[0].name, position, rotation);
            DontDestroyOnLoad(playerInstance);
            Debug.Log("동기화된 플레이어 생성 완료.");
        }
        else
        {
            // 기존 플레이어의 위치 및 회전값 동기화
            playerInstance.transform.position = position;
            playerInstance.transform.rotation = rotation;
            Debug.Log("플레이어 위치 및 회전 동기화 완료.");
        }
    }

    public void OnButtonPressed()
    {
        photonView.RPC("PlayerReady", RpcTarget.AllBuffered);
        Debug.Log($"현재 준비된 플레이어 수: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

        if (readyPlayerCount >= 2)
        {
            Debug.Log("2명 준비 완료! 다음 씬으로 이동합니다.");
            PhotonNetwork.LoadLevel("JHScenes3");
        }
    }

    [PunRPC]
    public void PlayerReady()
    {
        readyPlayerCount++;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");
    }
}
