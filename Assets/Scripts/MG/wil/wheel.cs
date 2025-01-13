using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class whlee : MonoBehaviour
{
    public Transform cameraRig; // ī�޶� ����

    public Transform leftWheel; // ���� ����
    public Transform rightWheel; // ������ ����

    [SerializeField] private float moveSpeed = 2.0f;          // �̵� �ӵ�
    [SerializeField] private float dampingFactor = 0.98f;     // ���� ���(����)
    [SerializeField] private float wheelRotationSpeed = 100f; // �� ȸ�� �ӵ�

    private Vector3 velocity = Vector3.zero; // ���� �̵� �ӵ�

    private InputDevice leftController;
    private InputDevice rightController;

    private void Start()
    {
        // VR ��Ʈ�ѷ� �ʱ�ȭ
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
        // �Է°� ó�� �� �̵�
        HandleMovement();

        // ����(����) ó��
        ApplyDamping();

        // �̵� ����
        Vector3 newPosition = cameraRig.position + velocity * Time.deltaTime;
        cameraRig.position = newPosition;
    }

    private void HandleMovement()
    {
        // ���� ��Ʈ�ѷ� �ӵ� ��������
        Vector3 leftControllerVelocity = GetControllerVelocity(leftController);
        // ������ ��Ʈ�ѷ� �ӵ� ��������
        Vector3 rightControllerVelocity = GetControllerVelocity(rightController);

        // ���� ��Ʈ�ѷ��� Z�� �̵��� ��� ���
        float forwardBackwardMovement = (leftControllerVelocity.z + rightControllerVelocity.z) / 2;

        // �ӵ� ������Ʈ (��Ʈ�ѷ��� �аų� ��� �������� �̵�)
        velocity += cameraRig.forward * forwardBackwardMovement * moveSpeed;

        // �� ȸ�� ó��
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
        // �̵� �ӵ� ũ�⸦ �������� �� ȸ�� �ӵ� ���
        float movementSpeed = Mathf.Abs(forwardBackwardMovement); // �ӵ� ũ�� ���
        float rotationSpeed = movementSpeed * wheelRotationSpeed;

        // ���� �� ȸ�� (��/���� ���⿡ ����)
        leftWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime * Mathf.Sign(forwardBackwardMovement));
        rightWheel.Rotate(Vector3.right, rotationSpeed * Time.deltaTime * Mathf.Sign(forwardBackwardMovement));
    }

    private void ApplyDamping()
    {
        // ���� ó��
        velocity *= dampingFactor;
    }
}


