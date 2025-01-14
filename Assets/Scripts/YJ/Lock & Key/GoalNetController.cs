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
        Debug.Log("�� �浹��");

        // �浹�� ������Ʈ�� 'Ball' �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Ball"))
        {
            // DoorController�� OpenDoor �޼��� ȣ�� �� ����ȭ
            if (doorController != null)
            {
                photonView.RPC("OpenLabDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {
        // DoorController�� OpenDoor �޼��� ȣ��
        if (doorController != null)
        {
            doorController.OpenDoor();
        }
    }
}
