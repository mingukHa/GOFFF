using UnityEngine;

public class LockController : MonoBehaviour
{
    // DoorController�� ������ �ʵ�
    public DoorController doorController;

    private Collider[] lockColliders;

    void Start()
    {
        // Lock ������Ʈ�� ���� Collider�� �����ɴϴ�
        lockColliders = GetComponentsInChildren<Collider>();

        // �� Collider�� Trigger �̺�Ʈ ó���� ���� ����
        foreach (var collider in lockColliders)
        {
            if (!collider.isTrigger)
            {
                collider.isTrigger = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� 'Key' �±׸� ������ �ִ��� Ȯ��
        if (other.CompareTag("Key"))
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
}
