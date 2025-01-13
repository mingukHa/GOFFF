using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllers : MonoBehaviour
{
    public Transform cameraRig; // ī�޶� ����

    public Transform leftWheel; // ���� ����
    public Transform rightWheel; // ������ ����

    [SerializeField] private InputActionProperty leftControllerVelocity;  // ���� ��Ʈ�ѷ� �ӵ�
    [SerializeField] private InputActionProperty rightControllerVelocity; // ������ ��Ʈ�ѷ� �ӵ�
    [SerializeField] private InputActionProperty leftControllerRotation;  // ���� ��Ʈ�ѷ� ȸ��
    [SerializeField] private InputActionProperty rightControllerRotation; // ������ ��Ʈ�ѷ� ȸ��
    [SerializeField] private InputActionProperty leftTrigger;             // ���� Ʈ���� �Է�
    [SerializeField] private InputActionProperty rightTrigger;            // ������ Ʈ���� �Է�
    [SerializeField] private InputActionProperty leftGrip;                // ���� �׸� �Է�
    [SerializeField] private InputActionProperty rightGrip;               // ������ �׸� �Է�

    [SerializeField] private float moveSpeed = 1.0f;          // �̵� �ӵ�
    [SerializeField] private float accelerationFactor = 2.0f; // ���ӵ� ���
    [SerializeField] private float dampingFactor = 0.99f;     // ���� ���(����)
    [SerializeField] private float wheelRotationSpeed = 100f; // �� ȸ�� �ӵ�

    private Vector3 velocity = Vector3.zero; // ���� �̵� �ӵ�
    private Quaternion initialLeftControllerRotation;  // ���� ��Ʈ�ѷ� �ʱ� ȸ��
    private Quaternion initialRightControllerRotation; // ������ ��Ʈ�ѷ� �ʱ� ȸ��
    private bool isLeftRotating = false;  // ���� ȸ�� ����
    private bool isRightRotating = false; // ������ ȸ�� ����

    private void Update()
    {
        // �� ��Ʈ�ѷ��� �Է��� �����Ͽ� �̵� ó��
        bool isMoving = HandleMovement();

        bool leftRotated = false;
        bool rightRotated = false;

        if (!isMoving)
        {
            // ������ ��Ʈ�ѷ��� ȸ�� ó��
            // ���� ��Ʈ�ѷ�: ��ȸ����
            leftRotated = HandleRotation(leftControllerRotation, ref initialLeftControllerRotation, ref isLeftRotating, true);
            // ������ ��Ʈ�ѷ�: ��ȸ����
            rightRotated = HandleRotation(rightControllerRotation, ref initialRightControllerRotation, ref isRightRotating, false);
        }

        // �� ȸ�� ����ȭ
        SyncWheelRotation(leftRotated, rightRotated);

        // ����(����) ó�� �Ǵ� �극��ũ ó��
        ApplyBrakingOrDamping(isMoving);

        // �̵� ����
        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;

        cameraRig.position = newPosition;
    }

    private bool HandleMovement()
    {
        // ���� ��Ʈ�ѷ� �Է� ó��
        Vector3 leftControllerMovement = leftControllerVelocity.action.ReadValue<Vector3>();
        Vector3 rightControllerMovement = rightControllerVelocity.action.ReadValue<Vector3>();

        // �� ��Ʈ�ѷ��� �Է��� ����
        Vector3 combinedMovement = leftControllerMovement + rightControllerMovement;
        float acceleration = combinedMovement.magnitude * accelerationFactor;

        // ���� ��Ʈ�ѷ��� �Է°� ���� -0.1f���� ������ �Ĺ� �̵�
        if (combinedMovement.z < -0.1f)
        {
            velocity += cameraRig.forward * (-combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // �̵� ��
        }
        // ���� ��Ʈ�ѷ��� �Է°� ���� 0.1f���� ũ�� ���� �̵�
        else if (combinedMovement.z > 0.1f)
        {
            velocity -= cameraRig.forward * (combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // �̵� ��
        }

        return false; // ����
    }

    private bool HandleRotation(InputActionProperty controllerRotation, ref Quaternion initialRotation, ref bool isRotating, bool isRightDirectionOnly)
    {
        bool hasRotated = false;

        // Rotation ���� ������Ʈ�� �� ����
        if (controllerRotation.action.triggered)
        {
            if (!isRotating)
            {
                initialRotation = Quaternion.Euler(controllerRotation.action.ReadValue<Vector3>());
                isRotating = true; // ȸ�� ��
            }

            // ���� ȸ����
            Quaternion currentRotation = Quaternion.Euler(controllerRotation.action.ReadValue<Vector3>());
            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(initialRotation);

            // Yaw (y�� ȸ��) ��ȭ ���
            float yawDelta = Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y);

            // ���⿡ ���� ȸ�� ó��
            if ((isRightDirectionOnly && yawDelta > 0) || (!isRightDirectionOnly && yawDelta < 0))
            {
                Vector3 pivot = isRightDirectionOnly ? leftWheel.position : rightWheel.position;
                cameraRig.RotateAround(pivot, Vector3.up, yawDelta);
                hasRotated = true;
            }

            initialRotation = currentRotation;
        }
        else
        {
            isRotating = false; // ȸ�� ����
        }

        return hasRotated;
    }

    private void SyncWheelRotation(bool leftRotated, bool rightRotated)
    {
        float movementSpeed = velocity.magnitude; // �ӵ� ũ�� ���
        float rotationSpeed = movementSpeed * wheelRotationSpeed;

        // ���� �� ȸ�� (��/����)
        leftWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        rightWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        if (leftRotated)
        {
            rightWheel.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }

        if (rightRotated)
        {
            leftWheel.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyBrakingOrDamping(bool isMoving)
    {
        if (isMoving)
        {
            return; // �̵� ���� ��� �극��ũ ȿ���� �������� ����
        }

        if ((leftTrigger.action.ReadValue<float>() > 0 || rightTrigger.action.ReadValue<float>() > 0) && velocity.magnitude < 0.5f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.1f); // �극��ũ
        }
        else
        {
            velocity *= dampingFactor; // �ڿ������� ����(����)
        }
    }
}


