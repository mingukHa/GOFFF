using UnityEngine;

public class HandleRotation : MonoBehaviour
{
    public Transform handleBase; // �ڵ��� ���� ��ġ
    private bool isGrabbed = false;
    private Transform grabbingHand;

    private Vector3 initialHandPosition;
    private float initialHandleRotation;

    void Update()
    {
        if (isGrabbed)
        {
            // ���� ����� �������� ���
            Vector3 handOffset = grabbingHand.position - initialHandPosition;

            // Z�� �������� ȸ�� ���
            float rotationDelta = Vector3.Dot(handOffset, handleBase.right) * 360f;

            // �ڵ� ȸ�� ����
            transform.localEulerAngles = new Vector3(0, 0, initialHandleRotation + rotationDelta);
        }
    }

    public void OnGrab(Transform hand)
    {
        isGrabbed = true;
        grabbingHand = hand;

        // �ʱ� ��ġ �� ȸ���� ����
        initialHandPosition = hand.position;
        initialHandleRotation = transform.localEulerAngles.z;
    }

    public void OnRelease()
    {
        isGrabbed = false;
        grabbingHand = null;
    }
}