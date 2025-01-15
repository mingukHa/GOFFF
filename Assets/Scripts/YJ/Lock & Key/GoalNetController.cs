using UnityEngine;
using Photon.Pun;

public class GoalNetController : MonoBehaviourPun
{
    public DoorController doorController; // ���� ���Ʈ�� ������ ���� ��
    private Collider goalNetCollider; // ���Ʈ�� ������ �ݶ��̴�

    private void Start()
    {
        goalNetCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        // �浹�� ������Ʈ�� 'Ball' �±׸� ������ �ִ��� Ȯ��
        if (collider.CompareTag("Ball"))
        {
            // RPC�� OpenLabDoor �޼ҵ� ȣ��
            if (doorController != null)
            {
                photonView.RPC("OpenLabDoor", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {

        if (doorController != null)
        {
            // DoorController�� OpenDoor �޼ҵ� ȣ���Ͽ� �� ����
            doorController.OpenDoor();
        }
    }
}
