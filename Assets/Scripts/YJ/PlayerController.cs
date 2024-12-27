using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig; // �÷��̾��� Transform
    public float moveSpeed = 1.0f; // �⺻ �̵� �ӵ�
    public float accelerationFactor = 1.8f; // �� ��Ʈ�ѷ� ��� �� ���ӵ� ���
    public float inertiaFactor = 0.05f; // Ʈ���Ÿ� �� �� �̵� ���� (������ ���ߴ� ����)
    public float maxSpeed = 5.0f; // �ִ� �̵� �ӵ�
    public float rotationSpeed = 100f; // ȸ�� �ӵ�

    private Vector3 leftVelocity; // ���� ��Ʈ�ѷ� �ӵ�
    private Vector3 rightVelocity; // ������ ��Ʈ�ѷ� �ӵ�
    private bool isLIdxTriggerPressed; // ���� �ε��� Ʈ���� ����
    private bool isRIdxTriggerPressed; // ������ �ε��� Ʈ���� ����
    private Vector3 leftMovementVelocity; // ���� ���� �ӵ�
    private Vector3 rightMovementVelocity; // ������ ���� �ӵ�

    private Vector3 leftPosition; // ���� ��Ʈ�ѷ� ��ġ
    private Vector3 rightPosition; // ������ ��Ʈ�ѷ� ��ġ
    private bool isLGripTriggerPressed; // ���� �׸� Ʈ���� ����
    private bool isRGripTriggerPressed; // ������ �׸� Ʈ���� ����

    private Vector3 lastLeftPosition; // ���� ��Ʈ�ѷ��� ������ ��ġ
    private Vector3 lastRightPosition; // ������ ��Ʈ�ѷ��� ������ ��ġ

    private void Start()
    {
        lastLeftPosition = cameraRig.position; // ī�޶��� ��ġ�� �ʱ�ȭ
        lastRightPosition = cameraRig.position;
    }

    // Input System �ݹ� �޼���
    // ------------ �̵� ------------
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        leftVelocity = context.ReadValue<Vector3>();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        rightVelocity = context.ReadValue<Vector3>();
    }

    public void OnLeftIndexTrigger(InputAction.CallbackContext context)
    {
        isLIdxTriggerPressed = context.ReadValue<float>() > 0.5f;
    }

    public void OnRightIndexTrigger(InputAction.CallbackContext context)
    {
        isRIdxTriggerPressed = context.ReadValue<float>() > 0.5f;
    }

    // ------------ ȸ�� ------------

    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        leftPosition = context.ReadValue<Vector3>();
    }

    public void OnRotateRight(InputAction.CallbackContext context)
    {
        rightPosition = context.ReadValue<Vector3>();
    }

    public void OnLeftGripTrigger(InputAction.CallbackContext context)
    {
        isLGripTriggerPressed = context.ReadValue<float>() > 0.5f;
    }

    public void OnRightGripTrigger(InputAction.CallbackContext context)
    {
        isRGripTriggerPressed = context.ReadValue<float>() > 0.5f;
    }

    private void Update()
    {
        // ���� �����Ӱ� ���� �̵� �Ÿ� ���
        Vector3 leftDelta = leftVelocity - lastLeftPosition;
        Vector3 rightDelta = rightVelocity - lastRightPosition;

        // ���� ��ġ ������Ʈ
        lastLeftPosition = leftVelocity;
        lastRightPosition = rightVelocity;

        // �̵� ���� ���
        Vector3 movement = Vector3.zero;

        if (isLIdxTriggerPressed)
        {
            movement += CalculateMovement(leftVelocity, leftDelta);
            leftMovementVelocity = CalculateMovement(leftVelocity, leftDelta); // Ʈ���Ÿ� ���� �� �ӵ� ���
        }
        else
        {
            // Ʈ���Ÿ� ���� �ӵ� ���� (����)
            leftMovementVelocity = Vector3.Lerp(leftMovementVelocity, Vector3.zero, inertiaFactor);
            movement += leftMovementVelocity;
        }

        if (isRIdxTriggerPressed)
        {
            movement += CalculateMovement(rightVelocity, rightDelta);
            rightMovementVelocity = CalculateMovement(rightVelocity, rightDelta); // Ʈ���Ÿ� ���� �� �ӵ� ���
        }
        else
        {
            // Ʈ���Ÿ� ���� �ӵ� ���� (����)
            rightMovementVelocity = Vector3.Lerp(rightMovementVelocity, Vector3.zero, inertiaFactor);
            movement += rightMovementVelocity;
        }

        // �� ��Ʈ�ѷ� ��� ��� �� ���� ����
        if (isLIdxTriggerPressed && isRIdxTriggerPressed)
        {
            movement *= accelerationFactor;
        }

        // ȸ�� ó��
        HandleRotation();

        // ī�޶� ���� �̵�
        cameraRig.position += movement * Time.deltaTime;

        // �ִ� �ӵ� ����
        if (movement.magnitude > maxSpeed)
        {
            movement = movement.normalized * maxSpeed;
        }
    }

    private void HandleRotation()
    {
        // �޼� Grip Trigger�� ��ȸ��
        if (isLGripTriggerPressed)
        {
            Vector3 leftDeltaPosition = leftPosition - lastLeftPosition;
            if (leftDeltaPosition.x > 0) // ���������� �������� ���� ��ȸ�� ���
            {
                float rotationInput = leftDeltaPosition.x; // �޼� ��Ʈ�ѷ��� X�� ������
                Rotate(rotationInput); // �ð� �������� ȸ��
            }
        }

        // ������ Grip Trigger�� ��ȸ��
        if (isRGripTriggerPressed)
        {
            Vector3 rightDeltaPosition = rightPosition - lastRightPosition;
            if (rightDeltaPosition.x < 0) // �������� �������� ���� ��ȸ�� ���
            {
                float rotationInput = rightDeltaPosition.x; // ������ ��Ʈ�ѷ��� X�� ������
                Rotate(rotationInput); // �ݽð� �������� ȸ��
            }
        }

        // ���� ��ġ ������Ʈ
        lastRightPosition = rightPosition;
        lastLeftPosition = leftPosition;
    }

    private Vector3 CalculateMovement(Vector3 controllerVelocity, Vector3 controllerDelta)
    {
        // ��Ʈ�ѷ��� �����ӿ� ���� �̵� ���� ���
        float forwardMovement = -controllerVelocity.z; // ��Ʈ�ѷ��� ������ ����
        Vector3 movementDirection = cameraRig.forward * (forwardMovement * moveSpeed);

        // �̵� �Ÿ� ������� ���ӵ� ����
        float distanceMoved = controllerDelta.magnitude;
        movementDirection *= (1 + distanceMoved); // �� ���� �����ϼ��� �� ���� �̵�

        return movementDirection;
    }

    private void Rotate(float direction)
    {
        // ȸ�� ����
        float rotationAmount = direction * rotationSpeed * Time.deltaTime;
        cameraRig.Rotate(Vector3.up, rotationAmount);
    }

    // SAVE POINT
}
