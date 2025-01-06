using UnityEngine;

public class WheelChairBalancer : MonoBehaviour
{
    public Transform leftWheel; // ���� ����
    public Transform rightWheel; // ������ ����
    public Transform centerOfMass; // ��ü���� �߽�
    private Rigidbody rb; // PlayerHolder�� Rigidbody
    public LayerMask groundLayer; // ���� ���̾�

    public float recoveryForce = 10f; // ������ ��� ���� ��
    public float uprightRotationSpeed = 5f; // ȸ�� ���� �ӵ�
    public float balanceCheckInterval = 0.5f; // ���� Ȯ�� �ֱ�
    public float groundCheckDistance = 0.5f; // ������ ���� �� �Ÿ�

    private bool isBalancing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        InvokeRepeating(nameof(CheckBalance), 0, balanceCheckInterval);
    }

    private void CheckBalance()
    {
        bool isLeftWheelOnGround = Physics.Raycast(leftWheel.position, Vector3.down, groundCheckDistance, groundLayer);
        bool isRightWheelOnGround = Physics.Raycast(rightWheel.position, Vector3.down, groundCheckDistance, groundLayer);

        if (!isLeftWheelOnGround && !isRightWheelOnGround && !isBalancing)
        {
            StartCoroutine(RecoverBalance());
        }
    }

    private System.Collections.IEnumerator RecoverBalance()
    {
        isBalancing = true;

        // ���� ��� ���� ����
        while (!IsUpright())
        {
            Vector3 uprightDirection = Vector3.up;
            Vector3 currentUp = transform.up;
            Vector3 correctionTorque = Vector3.Cross(currentUp, uprightDirection) * recoveryForce;

            rb.AddTorque(correctionTorque, ForceMode.Force);

            yield return null;
        }

        // ȸ�� ���� (����)
        Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * uprightRotationSpeed);
            yield return null;
        }

        isBalancing = false;
    }

    private bool IsUpright()
    {
        // �� ����� ���� �� ������ ���� ��ġ�ϴ��� Ȯ��
        return Vector3.Dot(transform.up, Vector3.up) > 0.98f;
    }
}
