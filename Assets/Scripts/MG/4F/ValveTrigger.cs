using Photon.Pun;
using UnityEngine;

public class ValveTrigger : MonoBehaviourPun
{
    public GameObject cage; //������ ���� ��
    
    private void OnCollisionEnter(Collision collision) //�ݸ��� ���Ͱ� �Ǹ�
    {
        if (collision.gameObject.CompareTag("Player")) //�÷��̾� �±� ������
        {
            CageDestroy(); //������ ��Ʈ����
            photonView.RPC("CageDestroy", RpcTarget.Others); //�ٸ� ������� �˸�
        }
    }
    [PunRPC]
    private void CageDestroy()
    {
        PhotonNetwork.Destroy(cage);
        SoundManager.instance.SFXPlay("JailCrack_SFX", this.gameObject);
    }

    
}
