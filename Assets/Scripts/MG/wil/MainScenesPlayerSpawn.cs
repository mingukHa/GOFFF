using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScenesPlayerSpawn : MonoBehaviourPun
{
    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹
    [SerializeField] private Transform[] spawnPoints; // 스폰 위치 배열
    private bool hasSpawned = false;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {//서버 연결 안 되어 있다면 로그인 씬으로 이동
            Debug.Log("포톤 서버와 연결이 안 되었음 로비로 이동");
            SceneManager.LoadScene("LoginScenes");
            return;
        }

        if (!PhotonNetwork.InRoom)
        {//방 입장 실패시
            Debug.Log("현재 방에 입장하지 않았습니다. 방 입장을 기다립니다...");
            return;
        }
        //코루틴으로 룸 입장 대기
        StartCoroutine(WaitForRoomReady());
    }

    //포톤 서버 연동을 기다린 후 플레이어를 생성
    private IEnumerator WaitForRoomReady()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        StartCoroutine(SpawnPlayerWithDelay());
    }
    //플레이어 프리팹을 일정 간격을 두고 생성
    private IEnumerator SpawnPlayerWithDelay()
    {
        // 각 플레이어의 ActorNumber를 기반으로 딜레이 설정
        float delay = (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 2f;
        Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName} 생성 딜레이: {delay}초");
        yield return new WaitForSeconds(delay); // 딜레이 후 생성

        SpawnPlayer();
        photonView.RPC("SpawnPlayer", RpcTarget.AllBuffered);
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

        // ActorNumber를 기반으로 스폰 포인트 선택
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];

        if (spawnPoint == null)
        {
            Debug.LogWarning($"스폰 포인트가 null입니다! Index: {playerIndex}. 기본 위치를 사용합니다.");
            spawnPoint = new GameObject("FallbackSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // 플레이어 생성
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
