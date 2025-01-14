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
        Debug.Log($"{other}�� �浹��");

        // �浹�� ������Ʈ�� 'Ball' �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Ball"))
        {
            
            // DoorController�� OpenDoor �޼��� ȣ�� �� ����ȭ
            if (doorController != null)
            {
                OpenLabDoor();
                photonView.RPC("OpenLabDoor", RpcTarget.Others);
            }
        }
    }

    [PunRPC]
    private void OpenLabDoor()
    {
        doorController.OpenDoor();
    }
}
