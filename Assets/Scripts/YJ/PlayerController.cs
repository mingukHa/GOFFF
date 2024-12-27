using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerOpenXR : MonoBehaviour
{
    public Transform cameraRig; // ī�޶� ����
    public Transform leftWheel; // ���� ����
    public Transform rightWheel; // ������ ����

    [SerializeField] private InputActionReference leftHandTrigger; // ���� �ڵ� Ʈ���� �Է�
    [SerializeField] private InputActionReference rightHandTrigger; // ������ �ڵ� Ʈ���� �Է�
    [SerializeField] private InputActionReference leftIndexTrigger; // ���� �ε��� Ʈ���� �Է�
    [SerializeField] private InputActionReference rightIndexTrigger; // ������ �ε��� Ʈ���� �Է�
    [SerializeField] private InputActionReference leftControllerVelocity; // ���� ��Ʈ�ѷ� �ӵ�
    [SerializeField] private InputActionReference rightControllerVelocity; // ������ ��Ʈ�ѷ� �ӵ�
    [SerializeField] private InputActionReference leftControllerRotation; // ���� ��Ʈ�ѷ� ȸ��
    [SerializeField] private InputActionReference rightControllerRotation; // ������ ��Ʈ�ѷ� ȸ��

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float accelerationFactor = 2.0f;
    [SerializeField] private float dampingFactor = 0.99f;
    [SerializeField] private float wheelRotationSpeed = 100f;

    private Vector3 velocity = Vector3.zero;
    private Quaternion initialLeftControllerRotation;
    private Quaternion initialRightControllerRotation;
    private bool isLeftRotating = false;
    private bool isRightRotating = false;

    private void Update()
    {
        bool isMoving = HandleMovement();

        bool leftRotated = false;
        bool rightRotated = false;

        if (!isMoving)
        {
            leftRotated = HandleRotation(leftControllerRotation, ref initialLeftControllerRotation, ref isLeftRotating, true);
            rightRotated = HandleRotation(rightControllerRotation, ref initialRightControllerRotation, ref isRightRotating, false);
        }

        SyncWheelRotation(leftRotated, rightRotated);
        ApplyBrakingOrDamping(isMoving);

        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;

        if (newPosition.z < 0)
        {
            newPosition.z = 0;
            velocity.z = Mathf.Max(0, velocity.z);
        }

        cameraRig.position = newPosition;
    }

    private bool HandleMovement()
    {
        Vector3 leftMovement = GetControllerMovement(leftHandTrigger, leftControllerVelocity);
        Vector3 rightMovement = GetControllerMovement(rightHandTrigger, rightControllerVelocity);

        Vector3 combinedMovement = leftMovement + rightMovement;
        float acceleration = combinedMovement.magnitude * accelerationFactor;

        if (combinedMovement.z < -0.1f)
        {
            velocity += cameraRig.forward * (-combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true;
        }
        else if (combinedMovement.z > 0.1f)
        {
            velocity -= cameraRig.forward * (combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true;
        }

        return false;
    }

    private Vector3 GetControllerMovement(InputActionReference trigger, InputActionReference velocity)
    {
        if (trigger.action.ReadValue<float>() > 0.1f)
        {
            return velocity.action.ReadValue<Vector3>();
        }
        return Vector3.zero;
    }

    private bool HandleRotation(InputActionReference rotationAction, ref Quaternion initialRotation, ref bool isRotating, bool isRightDirectionOnly)
    {
        bool hasRotated = false;

        // ���� ��Ʈ�ѷ��� ȸ������ InputAction���� ������
        Quaternion currentRotation = rotationAction.action.ReadValue<Quaternion>();

        // Ʈ���Ű� ���ȴ��� Ȯ�� (������ InputAction ���)
        if (rotationAction.action.WasPressedThisFrame())
        {
            // �ʱ� ȸ���� ���� �� ȸ�� ����
            initialRotation = currentRotation;
            isRotating = true;
        }
        else if (rotationAction.action.WasReleasedThisFrame())
        {
            // ȸ�� ���� ����
            isRotating = false;
        }

        // ȸ�� ������ ���� ó��
        if (isRotating)
        {
            // ȸ�� ��ȭ ���
            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(initialRotation);
            float yawDelta = Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y);

            // ����� �Է¿� ���� ȸ�� ó��
            if ((isRightDirectionOnly && yawDelta > 0) || (!isRightDirectionOnly && yawDelta < 0))
            {
                Vector3 pivot = isRightDirectionOnly ? leftWheel.position : rightWheel.position;
                cameraRig.RotateAround(pivot, Vector3.up, yawDelta);
                hasRotated = true;
            }

            // ���� �������� ���� �ʱ� ȸ���� ������Ʈ
            initialRotation = currentRotation;
        }

        return hasRotated;
    }


    private void SyncWheelRotation(bool leftRotated, bool rightRotated)
    {
        float forwardSpeed = velocity.z;
        float rotationSpeed = forwardSpeed * wheelRotationSpeed;

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
        if (isMoving) return;

        bool isLeftPressed = leftIndexTrigger.action.ReadValue<float>() > 0.1f;
        bool isRightPressed = rightIndexTrigger.action.ReadValue<float>() > 0.1f;

        if ((isLeftPressed || isRightPressed) && velocity.magnitude < 0.5f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.1f);
        }
        else
        {
            velocity *= dampingFactor;
        }
    }
}
