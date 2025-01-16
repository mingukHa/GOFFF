using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MonsterScene : MonoBehaviourPun
{
    [SerializeField] private string restartSceneName; // 다시 시작할 씬 이름, 비워두면 현재 씬 재시작
    [SerializeField] private GameObject player1StartPoint; // 첫 번째 플레이어 시작 위치 지정 오브젝트
    [SerializeField] private GameObject player2StartPoint; // 두 번째 플레이어 시작 위치 지정 오브젝트

    private void Update()
    {
        // 스페이스바를 누르면 씬 재시작
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RestartScene", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터가 "Player" 태그를 가진 객체와 충돌할 경우
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.SFXPlay("GameOver_SFX", this.gameObject);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RestartSceneWithDelay", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void RestartSceneWithDelay()
    {
        StartCoroutine(RestartSceneCoroutine());
    }

    private IEnumerator RestartSceneCoroutine()
    {
        // 1초 지연
        yield return new WaitForSeconds(0.5f);

        // 재시작할 씬 이름 설정
        string sceneToLoad = string.IsNullOrEmpty(restartSceneName)
            ? SceneManager.GetActiveScene().name
            : restartSceneName;

        // 씬 재시작
        SceneManager.LoadScene(sceneToLoad);

        // 코루틴으로 플레이어 위치 초기화
        StartCoroutine(ResetPlayersPositionAfterSceneLoad());
    }

    private IEnumerator ResetPlayersPositionAfterSceneLoad()
    {
        // 씬이 완전히 로드될 때까지 대기
        yield return new WaitForSeconds(0.1f);

        // 첫 번째 플레이어 찾기
        GameObject player1 = GameObject.Find("PlayerHolder(Clone)");
        if (player1 != null && player1StartPoint != null)
        {
            player1.transform.position = player1StartPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("PlayerHolder(Clone) or Start Point for Player 1 not found!");
        }

        // 두 번째 플레이어 찾기
        GameObject player2 = GameObject.Find("PlayerHolder1(Clone)");
        if (player2 != null && player2StartPoint != null)
        {
            player2.transform.position = player2StartPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("PlayerHolder1(Clone) or Start Point for Player 2 not found!");
        }
    }
}
