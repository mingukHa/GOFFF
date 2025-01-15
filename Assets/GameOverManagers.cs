using Photon.Pun;
using UnityEngine;

public class GameOverManagers : MonoBehaviourPun
{
    private GameObject Player1; // PlayerHolder
    private GameObject Player2; // PlayerHolder1

    [SerializeField] private Transform spwan1;
    [SerializeField] private Transform spwan2;

    private void Awake()
    {
        // �̸����� �÷��̾� ������Ʈ ã��
        Player1 = GameObject.Find("PlayerHolder");
        Player2 = GameObject.Find("PlayerHolder1");
        Debug.LogError($"{Player1},{Player2} Ȯ���� ��");
        if (Player1 == null)
        {
            Debug.LogError($"{Player1}Ȯ���� �� ��");
        }
        if (Player2 == null)
        {
            Debug.LogError($"{Player2}Ȯ���� �� ��");
        }
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // �÷��̾� ��ġ �ʱ�ȭ
            if (Player1 != null && Player2 != null)
            {
                Player1.transform.position = spwan1.position;
                Player2.transform.position = spwan2.position;
            }
            else
            {
                Debug.LogError("Player1 �Ǵ� Player2�� null�Դϴ�. ��ġ �ʱ�ȭ�� ������ �� �����ϴ�.");
            }

            // �� ���ε�
            string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel(SceneName);
        }
    }
}
