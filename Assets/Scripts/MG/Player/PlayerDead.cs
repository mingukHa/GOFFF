using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerBehavior : MonoBehaviourPun
{
    private NewGameOver gameOverManager; // NewGameOver ����
    private float overTime = 0.5f;
    private void Start()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ���� ������ NewGameOver ã��
        StartCoroutine(WaitForGameOverManager());
    }

    private void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ����Ǹ� ���ο� NewGameOver�� ã��
        Debug.Log($"�� ���� ����: {scene.name}");
        StartCoroutine(WaitForGameOverManager());
    }

    private IEnumerator WaitForGameOverManager()
    {
        while (gameOverManager == null)
        {
            Debug.Log("NewGameOver ���� ��õ� ��...");
            FindGameOverManager();
            yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� ��õ�
        }
        Debug.Log("NewGameOver ���� �Ϸ�.");
    }

    private void FindGameOverManager()
    {
        GameObject managerObject = GameObject.FindWithTag("GameOver"); // �±׷� ã��
        if (managerObject != null)
        {
            gameOverManager = managerObject.GetComponent<NewGameOver>();
            if (gameOverManager != null)
            {
                Debug.Log("NewGameOver ���� ���� �Ϸ�.");
            }
            else
            {
                Debug.LogError("NewGameOver ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("NewGameOver ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // ���� �÷��̾ ó��
        
        if (other.CompareTag("Monster"))
        {
            float currtime = Time.deltaTime;
            Debug.Log("���Ϳ� �浹 �߻�!");
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
            Debug.LogWarning("NewGameOver�� �������� �ʾҽ��ϴ�.");
        }
    }
}
