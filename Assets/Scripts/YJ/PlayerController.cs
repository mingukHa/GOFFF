using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerOpenXR : MonoBehaviour
{
    public Transform cameraRig; // 카메라 리그
    public Transform leftWheel; // 왼쪽 바퀴
    public Transform rightWheel; // 오른쪽 바퀴

    [SerializeField] private InputActionReference leftHandTrigger; // 왼쪽 핸드 트리거 입력
    [SerializeField] private InputActionReference rightHandTrigger; // 오른쪽 핸드 트리거 입력
    [SerializeField] private InputActionReference leftIndexTrigger; // 왼쪽 인덱스 트리거 입력
    [SerializeField] private InputActionReference rightIndexTrigger; // 오른쪽 인덱스 트리거 입력
    [SerializeField] private InputActionReference leftControllerVelocity; // 왼쪽 컨트롤러 속도
    [SerializeField] private InputActionReference rightControllerVelocity; // 오른쪽 컨트롤러 속도
    [SerializeField] private InputActionReference leftControllerRotation; // 왼쪽 컨트롤러 회전
    [SerializeField] private InputActionReference rightControllerRotation; // 오른쪽 컨트롤러 회전

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

        // 현재 컨트롤러의 회전값을 InputAction에서 가져옴
        Quaternion currentRotation = rotationAction.action.ReadValue<Quaternion>();

        // 트리거가 눌렸는지 확인 (적절한 InputAction 사용)
        if (rotationAction.action.WasPressedThisFrame())
        {
            // 초기 회전값 설정 및 회전 시작
            initialRotation = currentRotation;
            isRotating = true;
        }
        else if (rotationAction.action.WasReleasedThisFrame())
        {
            // 회전 상태 종료
            isRotating = false;
        }

        // 회전 상태일 때만 처리
        if (isRotating)
        {
            // 회전 변화 계산
            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(initialRotation);
            float yawDelta = Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y);

            // 방향과 입력에 따라 회전 처리
            if ((isRightDirectionOnly && yawDelta > 0) || (!isRightDirectionOnly && yawDelta < 0))
            {
                Vector3 pivot = isRightDirectionOnly ? leftWheel.position : rightWheel.position;
                cameraRig.RotateAround(pivot, Vector3.up, yawDelta);
                hasRotated = true;
            }

            // 다음 프레임을 위해 초기 회전값 업데이트
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
