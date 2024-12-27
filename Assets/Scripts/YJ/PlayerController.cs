using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig; // Player의 Transform
    public float moveSpeed = 1.0f; // 기본 이동 속도
    public float accelerationFactor = 2.0f; // 두 컨트롤러 사용 시 가속도 계수

    private Vector3 leftVelocity; // 왼쪽 컨트롤러 속도
    private Vector3 rightVelocity; // 오른쪽 컨트롤러 속도
    private bool isLeftTriggerPressed; // 왼쪽 트리거 상태
    private bool isRightTriggerPressed; // 오른쪽 트리거 상태

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
        // 이동 방향 계산
        Vector3 movement = Vector3.zero;

        if (isLeftTriggerPressed)
        {
            movement += CalculateMovement(leftVelocity);
        }

        if (isRightTriggerPressed)
        {
            movement += CalculateMovement(rightVelocity);
        }

        // 두 컨트롤러 모두 사용 시 가속 적용
        if (isLeftTriggerPressed && isRightTriggerPressed)
        {
            movement *= accelerationFactor;
        }

        // 이동 적용
        cameraRig.position += movement * Time.deltaTime;
    }

    private Vector3 CalculateMovement(Vector3 controllerVelocity)
    {
        // Z축 방향으로 이동 계산
        float forwardMovement = -controllerVelocity.z; // 컨트롤러 움직임 반전
        return cameraRig.forward * (forwardMovement * moveSpeed);
    }
}
