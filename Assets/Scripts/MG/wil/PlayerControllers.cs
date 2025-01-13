using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllers : MonoBehaviour
{
    public Transform cameraRig; // 카메라 리그

    public Transform leftWheel; // 왼쪽 바퀴
    public Transform rightWheel; // 오른쪽 바퀴

    [SerializeField] private InputActionProperty leftControllerVelocity;  // 왼쪽 컨트롤러 속도
    [SerializeField] private InputActionProperty rightControllerVelocity; // 오른쪽 컨트롤러 속도
    [SerializeField] private InputActionProperty leftControllerRotation;  // 왼쪽 컨트롤러 회전
    [SerializeField] private InputActionProperty rightControllerRotation; // 오른쪽 컨트롤러 회전
    [SerializeField] private InputActionProperty leftTrigger;             // 왼쪽 트리거 입력
    [SerializeField] private InputActionProperty rightTrigger;            // 오른쪽 트리거 입력
    [SerializeField] private InputActionProperty leftGrip;                // 왼쪽 그립 입력
    [SerializeField] private InputActionProperty rightGrip;               // 오른쪽 그립 입력

    [SerializeField] private float moveSpeed = 1.0f;          // 이동 속도
    [SerializeField] private float accelerationFactor = 2.0f; // 가속도 계수
    [SerializeField] private float dampingFactor = 0.99f;     // 감속 계수(관성)
    [SerializeField] private float wheelRotationSpeed = 100f; // 휠 회전 속도

    private Vector3 velocity = Vector3.zero; // 현재 이동 속도
    private Quaternion initialLeftControllerRotation;  // 왼쪽 컨트롤러 초기 회전
    private Quaternion initialRightControllerRotation; // 오른쪽 컨트롤러 초기 회전
    private bool isLeftRotating = false;  // 왼쪽 회전 상태
    private bool isRightRotating = false; // 오른쪽 회전 상태

    private void Update()
    {
        // 두 컨트롤러의 입력을 결합하여 이동 처리
        bool isMoving = HandleMovement();

        bool leftRotated = false;
        bool rightRotated = false;

        if (!isMoving)
        {
            // 각각의 컨트롤러로 회전 처리
            // 왼쪽 컨트롤러: 우회전만
            leftRotated = HandleRotation(leftControllerRotation, ref initialLeftControllerRotation, ref isLeftRotating, true);
            // 오른쪽 컨트롤러: 좌회전만
            rightRotated = HandleRotation(rightControllerRotation, ref initialRightControllerRotation, ref isRightRotating, false);
        }

        // 휠 회전 동기화
        SyncWheelRotation(leftRotated, rightRotated);

        // 감속(관성) 처리 또는 브레이크 처리
        ApplyBrakingOrDamping(isMoving);

        // 이동 적용
        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;

        cameraRig.position = newPosition;
    }

    private bool HandleMovement()
    {
        // 왼쪽 컨트롤러 입력 처리
        Vector3 leftControllerMovement = leftControllerVelocity.action.ReadValue<Vector3>();
        Vector3 rightControllerMovement = rightControllerVelocity.action.ReadValue<Vector3>();

        // 두 컨트롤러의 입력을 결합
        Vector3 combinedMovement = leftControllerMovement + rightControllerMovement;
        float acceleration = combinedMovement.magnitude * accelerationFactor;

        // 양쪽 컨트롤러의 입력값 합이 -0.1f보다 작으면 후방 이동
        if (combinedMovement.z < -0.1f)
        {
            velocity += cameraRig.forward * (-combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // 이동 중
        }
        // 양쪽 컨트롤러의 입력값 합이 0.1f보다 크면 전방 이동
        else if (combinedMovement.z > 0.1f)
        {
            velocity -= cameraRig.forward * (combinedMovement.z * moveSpeed * acceleration * Time.deltaTime);
            return true; // 이동 중
        }

        return false; // 정지
    }

    private bool HandleRotation(InputActionProperty controllerRotation, ref Quaternion initialRotation, ref bool isRotating, bool isRightDirectionOnly)
    {
        bool hasRotated = false;

        // Rotation 값이 업데이트될 때 동작
        if (controllerRotation.action.triggered)
        {
            if (!isRotating)
            {
                initialRotation = Quaternion.Euler(controllerRotation.action.ReadValue<Vector3>());
                isRotating = true; // 회전 중
            }

            // 현재 회전값
            Quaternion currentRotation = Quaternion.Euler(controllerRotation.action.ReadValue<Vector3>());
            Quaternion rotationDelta = currentRotation * Quaternion.Inverse(initialRotation);

            // Yaw (y축 회전) 변화 계산
            float yawDelta = Mathf.DeltaAngle(0, rotationDelta.eulerAngles.y);

            // 방향에 따라 회전 처리
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
            isRotating = false; // 회전 중지
        }

        return hasRotated;
    }

    private void SyncWheelRotation(bool leftRotated, bool rightRotated)
    {
        float movementSpeed = velocity.magnitude; // 속도 크기 계산
        float rotationSpeed = movementSpeed * wheelRotationSpeed;

        // 양쪽 휠 회전 (전/후진)
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
            return; // 이동 중인 경우 브레이크 효과를 적용하지 않음
        }

        if ((leftTrigger.action.ReadValue<float>() > 0 || rightTrigger.action.ReadValue<float>() > 0) && velocity.magnitude < 0.5f)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, 0.1f); // 브레이크
        }
        else
        {
            velocity *= dampingFactor; // 자연스러운 감속(관성)
        }
    }
}


