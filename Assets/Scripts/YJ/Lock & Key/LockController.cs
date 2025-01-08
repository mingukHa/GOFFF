using UnityEngine;
using Photon.Pun;

public class LockController : MonoBehaviourPun
{
    // DoorController�� ������ �ʵ�
    public DoorController doorController;
    private Collider lockColliders;

    private void Start()
    {
        lockColliders = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� 'Key' �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Key"))
        {
            // DoorController�� OpenDoor �޼��� ȣ�� �� ����ȭ
            if (doorController != null)
            {
                photonView.RPC("OpenDoorAndDestroy", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenDoorAndDestroy()
    {
        // DoorController�� OpenDoor �޼��� ȣ��
        if (doorController != null)
        {
            doorController.OpenDoor();
        }

        // Lock ������Ʈ ����
        Destroy(gameObject);
    }
}
