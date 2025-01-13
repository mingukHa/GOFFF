using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class whlee : MonoBehaviour
{
    public Transform cameraRig; // 카메라 리그

    public Transform leftWheel; // 왼쪽 바퀴
    public Transform rightWheel; // 오른쪽 바퀴

    [SerializeField] private float moveSpeed = 2.0f;          // 이동 속도
    [SerializeField] private float dampingFactor = 0.98f;     // 감속 계수(관성)
    [SerializeField] private float wheelRotationSpeed = 100f; // 휠 회전 속도

    private Vector3 velocity = Vector3.zero; // 현재 이동 속도

    private InputDevice leftController;
    private InputDevice rightController;

    private void Start()
    {
        // VR 컨트롤러 초기화
        var leftHandDevices = new List<InputDevice>();
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        if (leftHandDevices.Count > 0)
            leftController = leftHandDevices[0];
        else
            Debug.LogError("Left controller not found!");

        if (rightHandDevices.Count > 0)
            rightController = rightHandDevices[0];
        else
            Debug.LogError("Right controller not found!");
    }

    private void Update()
    {
        // 입력값 처리 및 이동
        HandleMovement();

        // 감속(관성) 처리
        ApplyDamping();

        // 이동 적용
        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;
        cameraRig.position = newPosition;
    }

    private void HandleMovement()
    {
        // 왼쪽 컨트롤러 속도 가져오기
        Vector3 leftControllerVelocity = GetControllerVelocity(leftController);
        // 오른쪽 컨트롤러 속도 가져오기
        Vector3 rightControllerVelocity = GetControllerVelocity(rightController);

        // 양쪽 컨트롤러의 Z축 이동값 평균 계산
        float forwardBackwardMovement = (leftControllerVelocity.z + rightControllerVelocity.z) / 2;

        // 속도 업데이트 (컨트롤러를 밀거나 당긴 방향으로 이동)
        velocity += cameraRig.forward * forwardBackwardMovement * moveSpeed;

        // 휠 회전 처리
        SyncWheelRotation(forwardBackwardMovement);
    }

    private Vector3 GetControllerVelocity(InputDevice controller)
    {
        if (controller.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            return velocity;
        }
        return Vector3.zero;
    }

    private void SyncWheelRotation(float forwardBackwardMovement)
    {
        // 이동 속도 크기를 기준으로 휠 회전 속도 계산
        float movementSpeed = Mathf.Abs(forwardBackwardMovement); // 속도 크기 계산
        float rotationSpeed = movementSpeed * wheelRotationSpeed;

        // 양쪽 휠 회전 (전/후진 방향에 따라)
        leftWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime * Mathf.Sign(forwardBackwardMovement));
        rightWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime * Mathf.Sign(forwardBackwardMovement));
    }

    private void ApplyDamping()
    {
        // 감속 처리
        velocity *= dampingFactor;
    }
}


