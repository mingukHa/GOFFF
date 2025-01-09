using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private Transform playerHolder; // 플레이어의 Transform
    public Transform leftWheel; // 휠체어 왼쪽 바퀴
    public Transform rightWheel; // 휠체어 오른쪽 바퀴

    public float moveSpeed = 1.0f; // 기본 이동 속도
    public float accelerationFactor = 1.8f; // 두 컨트롤러 사용 시 가속도 계수
    public float inertiaFactor = 0.05f; // 트리거를 뗀 후 이동 관성 (느리게 멈추는 정도)
    public float maxSpeed = 5.0f; // 최대 이동 속도
    public float rotationSpeed = 100f; // 회전 속도
    public float wheelRotationMultiplier = 50f; // 휠체어 바퀴 회전 계수

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
        playerHolder = GetComponent<Transform>();

        if (!photonView.IsMine)
        {
            // 다른 플레이어의 입력 및 컨트롤 비활성화
            this.enabled = false;
        }

        lastLeftPosition = playerHolder.position; // 카메라의 위치로 초기화
        lastRightPosition = playerHolder.position;
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

        Vector3 finalMovement = movement * Time.deltaTime;

        // 최대 속도 제한
        if (finalMovement.magnitude > maxSpeed)
        {
            finalMovement = finalMovement.normalized * maxSpeed;
        }


        // 카메라 리그 이동
        playerHolder.position += finalMovement;

        // 바퀴 회전 처리
        HandleWheelRotation(finalMovement.magnitude);

        // Photon 동기화
        photonView.RPC("SyncWheelRotation", RpcTarget.Others, finalMovement.magnitude);
    }

    private void HandleRotation()
    {
        float movementThreshold = 0.01f; // 움직임 감지 임계값

        // 왼손 Grip Trigger로 우회전
        if (isLGripTriggerPressed)
        {
            Vector3 leftDeltaPosition = leftPosition - lastLeftPosition;

            // Forward 방향으로 충분히 움직였을 때만 우회전 허용
            if (leftDeltaPosition.z > movementThreshold) // Z값 양수는 Forward 방향
            {
                float rotationInput = leftDeltaPosition.z; // 왼손 컨트롤러의 Z축 움직임
                Debug.Log($"{rotationInput}");
                ApplyRotation(rotationInput); // 시계 방향으로 회전
            }
        }

        // 오른손 Grip Trigger로 좌회전
        if (isRGripTriggerPressed)
        {
            Vector3 rightDeltaPosition = rightPosition - lastRightPosition;

            // Forward 방향으로 충분히 움직였을 때만 좌회전 허용
            if (rightDeltaPosition.z > movementThreshold) // Z값 양수는 Forward 방향
            {
                float rotationInput = -rightDeltaPosition.z; // 오른손 컨트롤러의 Z축 움직임
                ApplyRotation(rotationInput); // 반시계 방향으로 회전
                //Debug.Log($"{rotationInput}");
            }
        }

        // 이전 위치 업데이트
        lastRightPosition = rightPosition;
        lastLeftPosition = leftPosition;
    }


    private Vector3 CalculateMovement(Vector3 controllerVelocity, Vector3 controllerDelta)
    {
        // 컨트롤러의 움직임에 따라 이동 방향 계산
        float forwardMovement = controllerVelocity.z; // 컨트롤러의 z축 움직임
        Vector3 movementDirection = playerHolder.forward * (forwardMovement * moveSpeed);

        // 이동 거리 기반으로 가속도 적용
        float distanceMoved = controllerDelta.magnitude;
        movementDirection *= (1 + distanceMoved); // 더 많이 움직일수록 더 빨리 이동

        return movementDirection;
    }

    private void ApplyRotation(float direction)
    {
        // 회전 구현
        float rotationAmount = direction * rotationSpeed * Time.deltaTime;
        playerHolder.Rotate(Vector3.up, rotationAmount);
    }

    private void HandleWheelRotation(float movementMagnitude)
    {
        // 이동 방향에 따른 회전 방향 설정
        float wheelRotation = movementMagnitude * wheelRotationMultiplier;

        // 회전 중인지 확인
        float rotationAmount = (isLGripTriggerPressed || isRGripTriggerPressed) ? rotationSpeed * Time.deltaTime : 0;

        if (isLGripTriggerPressed && !isRGripTriggerPressed) // 우회전
        {
            leftWheel.Rotate(Vector3.right, wheelRotation);
            rightWheel.Rotate(Vector3.right, 0f);
        }
        else if (isRGripTriggerPressed && !isLGripTriggerPressed) // 좌회전
        {
            rightWheel.Rotate(Vector3.right, wheelRotation);
            leftWheel.Rotate(Vector3.right, 0f);
        }
        else
        {
            // 전진 또는 후진 시 양쪽 바퀴 회전
            leftWheel.Rotate(Vector3.right, wheelRotation);
            rightWheel.Rotate(Vector3.right, wheelRotation);
        }
    }


    [PunRPC]
    private void SyncWheelRotation(float movementMagnitude)
    {
        float wheelRotation = movementMagnitude * wheelRotationMultiplier;

        leftWheel.Rotate(Vector3.right, wheelRotation);
        rightWheel.Rotate(Vector3.right, wheelRotation);
    }

    // SAVE POINT
}
