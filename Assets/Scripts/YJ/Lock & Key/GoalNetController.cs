using UnityEngine;
using Photon.Pun;

public class GoalNetController : MonoBehaviourPun
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
        if (other.CompareTag("Ball"))
        {
            // DoorController�� OpenDoor �޼��� ȣ�� �� ����ȭ
            if (doorController != null)
            {
                photonView.RPC("OpenDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenDoor()
    {
        // DoorController�� OpenDoor �޼��� ȣ��
        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}
