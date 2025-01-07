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

    private IEnumerator WaitForRoomReady()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        StartCoroutine(SpawnPlayerWithDelay());
    }

    private IEnumerator SpawnPlayerWithDelay()
    {
        float delay = (PhotonNetwork.LocalPlayer.ActorNumber - 1) * 2f;
        Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName} 생성 딜레이: {delay}초");
        yield return new WaitForSeconds(delay);

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

        // 플레이어 생성
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        if (player != null)
        {
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 스폰되었습니다.");
            hasSpawned = true;

            // 컨트롤러 초기화
            InitializeController(player);
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }

        StartCoroutine(ReenableCollider(player));
    }

    private void InitializeController(GameObject player)
    {
        if (player == null) return;

        // 컨트롤러 활성화
        var controller = player.GetComponentInChildren<XRController>(); // XR 컨트롤러 컴포넌트 가져오기
        if (controller != null)
        {
            Debug.Log("컨트롤러가 정상적으로 활성화되었습니다.");
            controller.enabled = true;
        }
        else
        {
            Debug.LogWarning("컨트롤러 컴포넌트를 찾을 수 없습니다.");
        }

        // 소유권 확인
        var photonView = player.GetComponent<PhotonView>();
        if (photonView != null && !photonView.IsMine)
        {
            Debug.Log("소유권이 없으므로 소유권 요청 중...");
            photonView.RequestOwnership();
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
}
