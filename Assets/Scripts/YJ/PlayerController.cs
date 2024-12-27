using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraRig; // Player�� Transform
    public float moveSpeed = 1.0f; // �⺻ �̵� �ӵ�
    public float accelerationFactor = 2.0f; // �� ��Ʈ�ѷ� ��� �� ���ӵ� ���

    private Vector3 leftVelocity; // ���� ��Ʈ�ѷ� �ӵ�
    private Vector3 rightVelocity; // ������ ��Ʈ�ѷ� �ӵ�
    private bool isLeftTriggerPressed; // ���� Ʈ���� ����
    private bool isRightTriggerPressed; // ������ Ʈ���� ����

    // Input System �ݹ� �޼���
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
        // �̵� ���� ���
        Vector3 movement = Vector3.zero;

        if (isLeftTriggerPressed)
        {
            movement += CalculateMovement(leftVelocity);
        }

        if (isRightTriggerPressed)
        {
            movement += CalculateMovement(rightVelocity);
        }

        // �� ��Ʈ�ѷ� ��� ��� �� ���� ����
        if (isLeftTriggerPressed && isRightTriggerPressed)
        {
            movement *= accelerationFactor;
        }

        // �̵� ����
        cameraRig.position += movement * Time.deltaTime;
    }

    private Vector3 CalculateMovement(Vector3 controllerVelocity)
    {
        // Z�� �������� �̵� ���
        float forwardMovement = -controllerVelocity.z; // ��Ʈ�ѷ� ������ ����
        return cameraRig.forward * (forwardMovement * moveSpeed);
    }
}
