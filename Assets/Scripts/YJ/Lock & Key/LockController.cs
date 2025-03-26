using UnityEngine;
using Photon.Pun;

public class LockController : MonoBehaviourPun
{
    public DoorManager doorController; // ���谡 �ڹ��迡 ������ ���� ��
    private Collider lockCollider; // �ڹ��迡 ������ �ݶ��̴�

    private void Start()
    {
        lockCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �浹�� ������Ʈ�� 'Key' �±׸� ������ �ִ��� Ȯ��
        if (collider.CompareTag("Key"))
        {
            // RPC�� OpenDoorAndDestroy �޼ҵ� ȣ��
            if (doorController != null)
            {
                photonView.RPC("OpenDoorAndDestroy", RpcTarget.All, collider.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    private void OpenDoorAndDestroy(int keyViewID)
    {
        if (doorController != null)
        {
            // DoorController�� OpenDoor �޼ҵ� ȣ���Ͽ� �� ����
            doorController.OpenDoor();
        }

        // ���� ������Ʈ ����
        PhotonView keyPhotonView = PhotonView.Find(keyViewID);
        if (keyPhotonView != null)
        {
            Destroy(keyPhotonView.gameObject);
        }

        // �ڹ��� ������Ʈ ����
        Destroy(gameObject);
    }
}
