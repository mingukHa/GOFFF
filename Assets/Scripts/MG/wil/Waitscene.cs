using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private GameObject playerInstance; // 플레이어 객체 저장
    private bool hasSpawned = false;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon 서버와 연결이 안 되었습니다. 로비로 이동합니다.");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방에 입장하지 않았습니다.");
            return;
        }

        SpawnPlayer();

        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoints == null)
        {
            Debug.LogError("Player Prefab 또는 Spawn Points가 설정되지 않았습니다.");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        playerInstance = PhotonNetwork.Instantiate(playerPrefab[playerIndex % playerPrefab.Length].name, spawnPoint.position, spawnPoint.rotation);
        DontDestroyOnLoad(playerInstance); // 씬 전환 후 객체 유지

        hasSpawned = true;
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} 플레이어가 스폰되었습니다.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드 완료: {scene.name}. 동기화 시작.");

        // 씬 로드 후 RPC로 상태 동기화
        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", RpcTarget.AllBuffered, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }

    [PunRPC]
    private void SyncPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerInstance == null)
        {
            Debug.Log("플레이어 객체가 없으므로 동기화 실패.");
            return;
        }

        // 객체 위치 및 회전 동기화
        playerInstance.transform.position = position;
        playerInstance.transform.rotation = rotation;

        Debug.Log("플레이어 상태 동기화 완료.");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");

        // 새로운 플레이어에게 동기화 정보 전달
        if (playerInstance != null)
        {
            photonView.RPC("SyncPlayer", newPlayer, playerInstance.transform.position, playerInstance.transform.rotation);
        }
    }
}
