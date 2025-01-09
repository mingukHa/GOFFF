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
    private int readyPlayerCount = 0; // 준비 완료된 플레이어 수

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
            SpawnPlayer();
        }
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

        if (spawnPoint == null)
        {
            Debug.LogWarning($"스폰 포인트가 null입니다! Index: {playerIndex}. 기본 위치를 사용합니다.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;

            // 플레이어 오브젝트를 TagObject에 저장
            // PhotonNetwork.LocalPlayer.TagObject = player;
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }

        StartCoroutine(ReenableCollider(player));

        // 모든 클라이언트에 DontDestroyOnLoad 적용
        photonView.RPC("SetDontDestroyOnLoad", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }

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
        photonView.RPC("PlayerReady", RpcTarget.AllBuffered); // 모든 클라이언트에 플레이어 준비 상태 전달
        Debug.Log($"현재 준비된 플레이어 수: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

        // 모든 플레이어가 준비되었을 경우 다음 씬으로 전환
        if (readyPlayerCount == 2)
        {
            
            Debug.Log("2명 준비 완료! 다음 씬으로 이동합니다.");
            PhotonNetwork.LoadLevel("JHScenes3"); // 전환할 씬 이름으로 변경
            
        }
        else
        {
            return;
        }
        Debug.Log("버튼이 눌렸습니다");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
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
