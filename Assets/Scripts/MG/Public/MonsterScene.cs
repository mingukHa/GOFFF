using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MonsterScene : MonoBehaviourPun
{
    [SerializeField] private string restartSceneName; // �ٽ� ������ �� �̸�, ����θ� ���� �� �����
    [SerializeField] private GameObject player1StartPoint; // ù ��° �÷��̾� ���� ��ġ ���� ������Ʈ
    [SerializeField] private GameObject player2StartPoint; // �� ��° �÷��̾� ���� ��ġ ���� ������Ʈ

    private void Update()
    {
        // �����̽��ٸ� ������ �� �����
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
        // ���Ͱ� "Player" �±׸� ���� ��ü�� �浹�� ���
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
        // 1�� ����
        yield return new WaitForSeconds(0.5f);

        // ������� �� �̸� ����
        string sceneToLoad = string.IsNullOrEmpty(restartSceneName)
            ? SceneManager.GetActiveScene().name
            : restartSceneName;

        // �� �����
        SceneManager.LoadScene(sceneToLoad);

        // �ڷ�ƾ���� �÷��̾� ��ġ �ʱ�ȭ
        StartCoroutine(ResetPlayersPositionAfterSceneLoad());
    }

    private IEnumerator ResetPlayersPositionAfterSceneLoad()
    {
        // ���� ������ �ε�� ������ ���
        yield return new WaitForSeconds(0.1f);

        // ù ��° �÷��̾� ã��
        GameObject player1 = GameObject.Find("PlayerHolder(Clone)");
        if (player1 != null && player1StartPoint != null)
        {
            player1.transform.position = player1StartPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("PlayerHolder(Clone) or Start Point for Player 1 not found!");
        }

        // �� ��° �÷��̾� ã��
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
