using UnityEngine;
using Photon.Pun;

public class PlayerReinitializer : MonoBehaviourPun
{
    [SerializeField] private GameObject playerPrefab; // 플레이어 프리팹
    [SerializeField] private Transform[] spawnPoints; // 스폰 위치 배열
    private bool isReinitializing = false;

    public void ReinitializePlayer()
    {
        if (isReinitializing) return; // 이미 재초기화 중이면 중복 실행 방지

        Debug.Log("플레이어 재초기화 시작...");
        isReinitializing = true;

        // 현재 플레이어 오브젝트 제거
        RemoveLocalPlayer();

        // 초기화 대기 후 새로 생성
        StartCoroutine(Reinitialize());
    }

    private void RemoveLocalPlayer()
    {
        if (photonView.IsMine)
        {
            Debug.Log("로컬 플레이어 오브젝트 삭제 중...");
            PhotonNetwork.Destroy(photonView.gameObject);
        }
    }

    private System.Collections.IEnumerator Reinitialize()
    {
        // 약간의 딜레이를 두어 초기화 안정성 확보
        yield return new WaitForSeconds(1f);

        Debug.Log("새 플레이어 생성 중...");
        SpawnPlayer();

        // 초기화 완료 플래그 설정
        isReinitializing = false;
        Debug.Log("플레이어 재초기화 완료.");
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
            Debug.Log($"플레이어 {PhotonNetwork.LocalPlayer.NickName}이(가) 위치 {spawnPoint.position}에 재생성되었습니다.");
        }
        else
        {
            Debug.LogError("플레이어 프리팹 생성에 실패했습니다!");
        }
    }
}
