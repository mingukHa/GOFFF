using UnityEngine;

public class Valve : MonoBehaviour
{
    public Transform cylinderAttachPoint; // Cylinder�� ���갡 ���� ��ġ (Cylinder�� �� GameObject �߰� �ʿ�)

    private Rigidbody valveRigidbody;

    void Start()
    {
        valveRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cylinder")) // Cylinder�� �±׸� "Cylinder"�� ����
        {
            AttachToCylinder(collision.gameObject);
        }
    }

    void AttachToCylinder(GameObject cylinder)
    {
        // Rigidbody ��Ȱ��ȭ
        valveRigidbody.isKinematic = true;
        valveRigidbody.useGravity = false;

        // ���� ��ġ�� ȸ�� ����
        Transform attachPoint = cylinder.transform.Find("AttachPoint"); // Cylinder�� AttachPoint�� �ڽ����� ����
        if (attachPoint != null)
        {
            transform.position = attachPoint.position;
            transform.rotation = attachPoint.rotation;
        }
        else
        {
            Debug.LogWarning("AttachPoint�� Cylinder�� �����ϴ�.");
        }
    }
}