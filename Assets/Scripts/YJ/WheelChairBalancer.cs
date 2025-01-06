using UnityEngine;

public class WheelChairBalancer : MonoBehaviour
{
    public Transform leftWheel; // 왼쪽 바퀴
    public Transform rightWheel; // 오른쪽 바퀴
    public Transform centerOfMass; // 휠체어의 중심
    private Rigidbody rb; // PlayerHolder의 Rigidbody
    public LayerMask groundLayer; // 지면 레이어

    public float recoveryForce = 10f; // 균형을 잡기 위한 힘
    public float uprightRotationSpeed = 5f; // 회전 복구 속도
    public float balanceCheckInterval = 0.5f; // 균형 확인 주기
    public float groundCheckDistance = 0.5f; // 바퀴와 지면 간 거리

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

        // 물리 기반 균형 복구
        while (!IsUpright())
        {
            Vector3 uprightDirection = Vector3.up;
            Vector3 currentUp = transform.up;
            Vector3 correctionTorque = Vector3.Cross(currentUp, uprightDirection) * recoveryForce;

            rb.AddTorque(correctionTorque, ForceMode.Force);

            yield return null;
        }

        // 회전 보정 (강제)
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
        // 위 방향과 월드 업 방향이 거의 일치하는지 확인
        return Vector3.Dot(transform.up, Vector3.up) > 0.98f;
    }
}
