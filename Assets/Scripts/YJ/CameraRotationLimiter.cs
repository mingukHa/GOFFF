using UnityEngine;

public class CameraRotationLimiter : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // ī�޶� Ʈ������
    [SerializeField] private float minYRotation = -90f; // �ּ� Y�� ȸ�� (����)
    [SerializeField] private float maxYRotation = 90f;  // �ִ� Y�� ȸ�� (������)

    private void Update()
    {
        // ī�޶��� ���� ���� ȸ���� ������
        Vector3 currentRotation = cameraTransform.localEulerAngles;

        // Y�� ȸ������ ���� (0~360 ������ -180~180���� ��ȯ)
        float clampedYRotation = Mathf.Clamp(
            (currentRotation.y > 180) ? currentRotation.y - 360 : currentRotation.y,
            minYRotation, maxYRotation
        );

        // ���ѵ� ���� �ٽ� ���� ȸ���� ����
        cameraTransform.localEulerAngles = new Vector3(currentRotation.x, clampedYRotation, currentRotation.z);
    }
}
