using UnityEngine;
using Photon.Pun;

public class WalkieTalkie : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered");

            PhotonView playerPhotonView = other.GetComponent<PhotonView>();

            // ���� �÷��̾ ó��
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // ������ ����
                GameObject walkieTalkie =
                    PhotonNetwork.Instantiate(
                    "WalkieTalkie(1P)", // ������ �̸� (Resources ������ �־�� ��)
                    other.transform.position + Vector3.up, // �÷��̾� ���� ����
                    Quaternion.identity // �⺻ ȸ����
                    );

                // �����⸦ ȹ���ϰ� ����ҿ� ��ǥ�� ���
                photonView.RPC("RegisterSavePointRPC", RpcTarget.All, transform.position, transform.rotation);

                // �����⸦ ��Ȱ��ȭ�ϰų� �ı� (�ɼ�)
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    private void RegisterSavePointRPC(Vector3 position, Quaternion rotation)
    {
        SaveManager.Instance.RegisterSavePoint(photonView.OwnerActorNr, position, rotation);
    }
}