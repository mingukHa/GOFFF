using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig; // 플레이어의 Transform
    public float moveSpeed = 1.0f; // 기본 이동 속도
    public float accelerationFactor = 1.8f; // 두 컨트롤러 사용 시 가속도 계수
    public float inertiaFactor = 0.05f; // 트리거를 뗀 후 이동 관성 (느리게 멈추는 정도)
    public float maxSpeed = 5.0f; // 최대 이동 속도
    public float rotationSpeed = 100f; // 회전 속도

    private Vector3 leftVelocity; // 왼쪽 컨트롤러 속도
    private Vector3 rightVelocity; // 오른쪽 컨트롤러 속도
    private bool isLIdxTriggerPressed; // 왼쪽 인덱스 트리거 상태
    private bool isRIdxTriggerPressed; // 오른쪽 인덱스 트리거 상태
    private Vector3 leftMovementVelocity; // 왼쪽 관성 속도
    private Vector3 rightMovementVelocity; // 오른쪽 관성 속도

    private Vector3 leftPosition; // 왼쪽 컨트롤러 위치
    private Vector3 rightPosition; // 오른쪽 컨트롤러 위치
    private bool isLGripTriggerPressed; // 왼쪽 그립 트리거 상태
    private bool isRGripTriggerPressed; // 오른쪽 그립 트리거 상태

    private Vector3 lastLeftPosition; // 왼쪽 컨트롤러의 마지막 위치
    private Vector3 lastRightPosition; // 오른쪽 컨트롤러의 마지막 위치

    private void Start()
    {
        lastLeftPosition = cameraRig.position; // 카메라의 위치로 초기화
        lastRightPosition = cameraRig.position;
    }

    // Input System 콜백 메서드
    // ------------ 이동 ------------
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

    // ------------ 회전 ------------

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
        // 이전 프레임과 비교해 이동 거리 계산
        Vector3 leftDelta = leftVelocity - lastLeftPosition;
        Vector3 rightDelta = rightVelocity - lastRightPosition;

        // 이전 위치 업데이트
        lastLeftPosition = leftVelocity;
        lastRightPosition = rightVelocity;

        // 이동 방향 계산
        Vector3 movement = Vector3.zero;

        if (isLIdxTriggerPressed)
        {
            movement += CalculateMovement(leftVelocity, leftDelta);
            leftMovementVelocity = CalculateMovement(leftVelocity, leftDelta); // 트리거를 누를 때 속도 계산
        }
        else
        {
            // 트리거를 떼면 속도 감소 (관성)
            leftMovementVelocity = Vector3.Lerp(leftMovementVelocity, Vector3.zero, inertiaFactor);
            movement += leftMovementVelocity;
        }

        if (isRIdxTriggerPressed)
        {
            movement += CalculateMovement(rightVelocity, rightDelta);
            rightMovementVelocity = CalculateMovement(rightVelocity, rightDelta); // 트리거를 누를 때 속도 계산
        }
        else
        {
            // 트리거를 떼면 속도 감소 (관성)
            rightMovementVelocity = Vector3.Lerp(rightMovementVelocity, Vector3.zero, inertiaFactor);
            movement += rightMovementVelocity;
        }

        // 두 컨트롤러 모두 사용 시 가속 적용
        if (isLIdxTriggerPressed && isRIdxTriggerPressed)
        {
            movement *= accelerationFactor;
        }

        // 회전 처리
        HandleRotation();

        // 카메라 리그 이동
        cameraRig.position += movement * Time.deltaTime;

        // 최대 속도 제한
        if (movement.magnitude > maxSpeed)
        {
            movement = movement.normalized * maxSpeed;
        }
    }

    private void HandleRotation()
    {
        // 왼손 Grip Trigger로 우회전
        if (isLGripTriggerPressed)
        {
            Vector3 leftDeltaPosition = leftPosition - lastLeftPosition;
            if (leftDeltaPosition.x > 0) // 오른쪽으로 움직였을 때만 우회전 허용
            {
                float rotationInput = leftDeltaPosition.x; // 왼손 컨트롤러의 X축 움직임
                Rotate(rotationInput); // 시계 방향으로 회전
            }
        }

        // 오른손 Grip Trigger로 좌회전
        if (isRGripTriggerPressed)
        {
            Vector3 rightDeltaPosition = rightPosition - lastRightPosition;
            if (rightDeltaPosition.x < 0) // 왼쪽으로 움직였을 때만 좌회전 허용
            {
                float rotationInput = rightDeltaPosition.x; // 오른손 컨트롤러의 X축 움직임
                Rotate(rotationInput); // 반시계 방향으로 회전
            }
        }

        // 이전 위치 업데이트
        lastRightPosition = rightPosition;
        lastLeftPosition = leftPosition;
    }

    private Vector3 CalculateMovement(Vector3 controllerVelocity, Vector3 controllerDelta)
    {
        // 컨트롤러의 움직임에 따라 이동 방향 계산
        float forwardMovement = -controllerVelocity.z; // 컨트롤러의 움직임 반전
        Vector3 movementDirection = cameraRig.forward * (forwardMovement * moveSpeed);

        // 이동 거리 기반으로 가속도 적용
        float distanceMoved = controllerDelta.magnitude;
        movementDirection *= (1 + distanceMoved); // 더 많이 움직일수록 더 빨리 이동

        return movementDirection;
    }

    private void Rotate(float direction)
    {
        // 회전 구현
        float rotationAmount = direction * rotationSpeed * Time.deltaTime;
        cameraRig.Rotate(Vector3.up, rotationAmount);
    }

    // SAVE POINT
}
