using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig; // 플레이어의 Transform
    public float moveSpeed = 1.0f; // 기본 이동 속도
    public float accelerationFactor = 1.8f; // 두 컨트롤러 사용 시 가속도 계수
    public float inertiaFactor = 0.05f; // 트리거를 뗀 후 이동 관성 (느리게 멈추는 정도)
    public float maxSpeed = 5.0f; // 최대 이동 속도

    private Vector3 leftVelocity; // 왼쪽 컨트롤러 속도
    private Vector3 rightVelocity; // 오른쪽 컨트롤러 속도
    private bool isLeftTriggerPressed; // 왼쪽 트리거 상태
    private bool isRightTriggerPressed; // 오른쪽 트리거 상태
    private Vector3 leftMovementVelocity; // 왼쪽 관성 속도
    private Vector3 rightMovementVelocity; // 오른쪽 관성 속도

    private Vector3 lastLeftPosition; // 왼쪽 컨트롤러의 마지막 위치
    private Vector3 lastRightPosition; // 오른쪽 컨트롤러의 마지막 위치

    private void Start()
    {
        lastLeftPosition = cameraRig.position; // 카메라의 위치로 초기화
        lastRightPosition = cameraRig.position;
    }

    // Input System 콜백 메서드
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        leftVelocity = context.ReadValue<Vector3>();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        rightVelocity = context.ReadValue<Vector3>();
    }

    public void OnLeftTrigger(InputAction.CallbackContext context)
    {
        isLeftTriggerPressed = context.ReadValue<float>() > 0.5f;
    }

    public void OnRightTrigger(InputAction.CallbackContext context)
    {
        isRightTriggerPressed = context.ReadValue<float>() > 0.5f;
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

        if (isLeftTriggerPressed)
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

        if (isRightTriggerPressed)
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
        if (isLeftTriggerPressed && isRightTriggerPressed)
        {
            movement *= accelerationFactor;
        }

        // 카메라 리그 이동
        cameraRig.position += movement * Time.deltaTime;

        // 최대 속도 제한
        if (movement.magnitude > maxSpeed)
        {
            movement = movement.normalized * maxSpeed;
        }
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

    // SAVE POINT
}
