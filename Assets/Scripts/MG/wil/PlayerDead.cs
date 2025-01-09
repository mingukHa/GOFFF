using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Invoke(nameof(SendRestartRPC), 2f); // 2�� �� RPC ȣ��
        }
    }

    private void SendRestartRPC()
    {
        // ��� Ŭ���̾�Ʈ�� �� ���ε� ��û
        photonView.RPC("ReStart", RpcTarget.Others);
    }

    [PunRPC]
    private void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel(SceneName);
        }
        else
        {
            Debug.Log("�� �ε�� ������ Ŭ���̾�Ʈ���� ó���˴ϴ�.");
        }
    }
}
