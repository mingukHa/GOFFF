using UnityEngine;

// Valve ���� ��ũ��Ʈ
public class Valve : MonoBehaviour
{
    public Transform cylinderAttachPoint;  // ��갡 �Ǹ����� ���� ��ġ ����
    private Rigidbody valveRigidbody;  // ����� Rigidbody�� ����
    private bool isAttached = false;  // ��갡 �Ǹ����� �پ����� �Ǻ��ϴ� ����
    private bool isRotating = false;  // ��갡 ȸ�� �Ǻ��ϴ� ����
    private float rotationSpeed = 10f;  // ��� ȸ�� �ӵ�
    private float currentRotationZ = 0f;  // ����� ���� z�� ȸ����

    public GameObject bridge1;  // ȸ���ϴ� �ٸ� ����1
    public GameObject bridge2;  // ȸ���ϴ� �ٸ� ����2

    void Start()
    {
        // Rigidbody ������Ʈ�� �����ͼ� valveRigidbody ������ ����
        valveRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // isRotating�� true�̰�, currentRotationZ�� 360�� �̸��� ���� ȸ�� ������Ʈ
        if (isRotating && currentRotationZ < 360f)
        {
            currentRotationZ += rotationSpeed * Time.deltaTime;  // ȸ�� ���� ������Ʈ (Time.deltaTime�� ���ؼ� ������ ���������� ȸ��)
            if (currentRotationZ >= 360f) currentRotationZ = 360f;  // ȸ�� ���� 360���� ���� �ʵ��� ����

            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);  // z�� ȸ������ �����Ͽ� ����� ȸ�� ������Ʈ
            if (bridge1 != null)  // bridge1�� null�� �ƴϸ�
            {
                // currentRotationZ�� ����Ͽ� bridge1�� ȸ�� ���� ����ϰ� ����
                float bridge1RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge1.transform.rotation = Quaternion.Euler(bridge1RotationX, 0, 0);  // ���� ȸ������ ����
            }

            if (bridge2 != null)  // bridge2�� null�� �ƴϸ�
            {
                // currentRotationZ�� ����Ͽ� bridge2�� ȸ�� ���� ����ϰ� ����
                float bridge2RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge2.transform.rotation = Quaternion.Euler(bridge2RotationX, 0, 0);  // ���� ȸ������ ����
            }
        }
    }

    // �ٸ� Collider�� �� ���� �浹���� �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "Cylinder" �±׸� ������ �ְ�, ��갡 ���� �Ǹ����� ���� �ʾҴٸ�
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            AttachToCylinder(other.gameObject);  // �Ǹ����� ��긦 ���̴� �޼��� ȣ��
        }
    }

    // �ٸ� Collider�� �浹�� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerExit(Collider other)
    {
        // �浹�� ������Ʈ�� "Cylinder" �±׸� ������ �ְ�, ��갡 �Ǹ����� �پ� �ִٸ�
        if (other.CompareTag("Cylinder") && isAttached)
        {
            DetachFromCylinder();  // �Ǹ������� ��긦 ���� �޼��� ȣ��
        }
    }

    // �Ǹ����� ��긦 ���̴� �޼���
    private void AttachToCylinder(GameObject cylinder)
    {
        if (isAttached) return;  // �̹� ��갡 �Ǹ����� �پ� �ִٸ� �ƹ� �۾��� ���� ����

        // Rigidbody ��Ȱ��ȭ (�߷� ������ ���� �ʵ���)
        valveRigidbody.isKinematic = true;
        valveRigidbody.useGravity = false;

        // �Ǹ����� AttachPoint ��ġ�� ã�Ƽ�, �� ��ġ�� ȸ�� ������ ��긦 ����
        Transform attachPoint = cylinder.transform.Find("AttachPoint");
        if (attachPoint != null)
        {
            transform.position = attachPoint.position;  // ����� ��ġ�� AttachPoint ��ġ�� ����
            transform.rotation = attachPoint.rotation;  // ����� ȸ���� AttachPoint ȸ������ ����
            isAttached = true;  // ��갡 �Ǹ����� �پ� �ִٰ� ǥ��
        }
    }

    // �Ǹ������� ��긦 ���� �޼���
    private void DetachFromCylinder()
    {
        if (!isAttached) return;  // ��갡 �Ǹ����� �پ� ���� �ʴٸ� �ƹ� �۾��� ���� ����

        // Rigidbody ���� ���·� ���� (�߷� ������ �޵��� ����)
        valveRigidbody.isKinematic = false;
        valveRigidbody.useGravity = true;
        isAttached = false;  // ��갡 �Ǹ������� �������ٰ� ǥ��
    }

    // �ܺο��� ȸ���� �����ϴ� �޼���
    public void StartRotation()
    {
        isRotating = true;  // ȸ�� ����
    }

    // �ܺο��� ȸ���� ���ߴ� �޼���
    public void StopRotation()
    {
        isRotating = false;  // ȸ�� ����
    }
}