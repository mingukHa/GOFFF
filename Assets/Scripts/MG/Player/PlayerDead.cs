using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerBehavior : MonoBehaviourPun
{
    private NewGameOver gameOverManager; // NewGameOver 참조
    private float overTime = 0.5f;
    private void Start()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 현재 씬에서 NewGameOver 찾기
        StartCoroutine(WaitForGameOverManager());
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 변경되면 새로운 NewGameOver를 찾음
        Debug.Log($"씬 변경 감지: {scene.name}");
        StartCoroutine(WaitForGameOverManager());
    }

    private IEnumerator WaitForGameOverManager()
    {
        while (gameOverManager == null)
        {
            Debug.Log("NewGameOver 참조 재시도 중...");
            FindGameOverManager();
            yield return new WaitForSeconds(0.5f); // 0.5초마다 재시도
        }
        Debug.Log("NewGameOver 참조 완료.");
    }

    private void FindGameOverManager()
    {
        GameObject managerObject = GameObject.FindWithTag("GameOver"); // 태그로 찾기
        if (managerObject != null)
        {
            gameOverManager = managerObject.GetComponent<NewGameOver>();
            if (gameOverManager != null)
            {
                Debug.Log("NewGameOver 참조 갱신 완료.");
            }
            else
            {
                Debug.LogError("NewGameOver 스크립트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("NewGameOver 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // 로컬 플레이어만 처리
        
        if (other.CompareTag("Monster"))
        {
            float currtime = Time.deltaTime;
            Debug.Log("몬스터와 충돌 발생!");
            if (overTime >= currtime)
            {
                photonView.RPC("HandlePlayerCollision", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void HandlePlayerCollision()
    {
        SoundManager.instance?.SFXPlay("GameOver_SFX", gameObject);

        if (gameOverManager != null)
        {
            gameOverManager.ReStart();
        }
        else
        {
            Debug.LogWarning("NewGameOver가 설정되지 않았습니다.");
        }
    }
}
