using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster")) //�ݶ��̴��� ���Ϳ� ������
        {
            photonView.RPC(nameof(NotifyDeath), RpcTarget.All); //��ο��� RPC�� ���
        }
    }

    [PunRPC]
    public void NotifyDeath()
    {
        if (PhotonNetwork.IsMasterClient)  //������ Ŭ���̾�Ʈ��
        {
            Invoke(nameof(RestartLevel), 2f); //2�� �� �Լ� ����
        }
    }

    private void RestartLevel()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; //���� �� �̸��� �����ϰ�
        PhotonNetwork.LoadLevel(sceneName); //���� �ҷ��´�
    }
}
