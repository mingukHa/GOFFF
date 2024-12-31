using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class Waitscene : MonoBehaviourPunCallbacks
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
    [PunRPC]
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        photonView.RPC("NotifyPlayerSpawned", newPlayer, PhotonNetwork.LocalPlayer.NickName);
        Debug.Log($"플레이어 {newPlayer.NickName}이(가) 방에 입장했습니다.");
    }


}
