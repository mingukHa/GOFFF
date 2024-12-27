using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig;
    public Transform leftWheel;
    public Transform rightWheel;

    // 입력 액션 정의
    public InputAction moveAction;
    public InputAction rotateLeftAction;
    public InputAction rotateRightAction;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float accelerationFactor = 2.0f;
    [SerializeField] private float dampingFactor = 0.99f;
    [SerializeField] private float wheelRotationSpeed = 100f;

    private Vector3 velocity = Vector3.zero;
    private Quaternion initialLeftControllerRotation;
    private Quaternion initialRightControllerRotation;
    private bool isLeftRotating = false;
    private bool isRightRotating = false;

    private void OnEnable()
    {
        // 입력 액션 활성화
        moveAction.Enable();
        rotateLeftAction.Enable();
        rotateRightAction.Enable();
    }

    private void OnDisable()
    {
        // 입력 액션 비활성화
        moveAction.Disable();
        rotateLeftAction.Disable();
        rotateRightAction.Disable();
    }

    private void Update()
    {
        bool isMoving = HandleMovement();

        bool leftRotated = false;
        bool rightRotated = false;

        if (!isMoving)
        {
            leftRotated = HandleRotation(rotateLeftAction, ref initialLeftControllerRotation, ref isLeftRotating, true);
            rightRotated = HandleRotation(rotateRightAction, ref initialRightControllerRotation, ref isRightRotating, false);
        }

        SyncWheelRotation(leftRotated, rightRotated);
        ApplyBrakingOrDamping(isMoving);

        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;
        cameraRig.position = newPosition;
    }

    private bool HandleMovement()
    {
        // OpenXR 입력을 통해 이동 처리
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // 이동 처리
        Vector3 combinedMovement = new Vector3(moveInput.x, 0, moveInput.y); // moveInput은 벡터로 가정

        if (combinedMovement.z < -0.1f)
        {
            velocity += cameraRig.forward * (-combinedMovement.z * moveSpeed * accelerationFactor * Time.deltaTime);
            return true;
        }
        else if (combinedMovement.z > 0.1f)
        {
            velocity -= cameraRig.forward * (combinedMovement.z * moveSpeed * accelerationFactor * Time.deltaTime);
            return true;
        }

        return false;
    }

    private bool HandleRotation(InputAction rotateAction, ref Quaternion initialRotation, ref bool isRotating, bool isRightDirectionOnly)
    {
        bool hasRotated = false;

        if (rotateAction.triggered)
        {
            initialRotation = Quaternion.Euler(0, rotateAction.ReadValue<Vector2>().x, 0); // 예시, 실제 입력에 맞게 수정
            isRotating = true;
        }

        if (isRotating)
        {
            Quaternion currentRotation = Quaternion.Euler(0, rotateAction.ReadValue<Vector2>().x, 0);
            float yawDelta = Mathf.DeltaAngle(0, currentRotation.eulerAngles.y);

            if ((isRightDirectionOnly && yawDelta > 0) || (!isRightDirectionOnly && yawDelta < 0))
            {
                Vector3 pivot = isRightDirectionOnly ? leftWheel.position : rightWheel.position;
                cameraRig.RotateAround(pivot, Vector3.up, yawDelta);
                hasRotated = true;
            }

            initialRotation = currentRotation;
        }

        return hasRotated;
    }

    private void SyncWheelRotation(bool leftRotated, bool rightRotated)
    {
        float movementSpeed = velocity.magnitude;
        float rotationSpeed = movementSpeed * wheelRotationSpeed;

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

        if (velocity.magnitude < 0.5f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.1f);
        }
        else
        {
            velocity *= dampingFactor;
        }
    }
}
