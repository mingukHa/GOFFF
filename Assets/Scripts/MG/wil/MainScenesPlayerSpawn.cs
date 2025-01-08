using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
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
            // 자신의 플레이어를 스폰
            SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
            // RPC로 모든 클라이언트에 플레이어 스폰 동기화
            photonView.RPC("SpawnPlayer", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    private IEnumerator WaitForRoomReady()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    private void SpawnPlayer(int actorNumber)
    {
        // 이미 스폰된 경우 중복 생성 방지
        if (hasSpawned) return;

        int playerIndex = actorNumber - 1; // ActorNumber 기반으로 스폰 위치 선택
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }

        StartCoroutine(ReenableCollider(player));
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
}
