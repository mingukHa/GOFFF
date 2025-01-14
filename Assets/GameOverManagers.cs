using Photon.Pun;
using UnityEngine;

public class GameOverManagers : MonoBehaviourPun
{
    [SerializeField] private Transform spwan1;
    [SerializeField] private Transform spwan2;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // �÷��̾� ��ġ �ʱ�ȭ
            Player1.transform.position = spwan1.position;
            Player2.transform.position = spwan2.position;

            // �� ���ε�
            string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel(SceneName);
        }
    }
}
