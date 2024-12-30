using UnityEngine;

public class HandleRotation : MonoBehaviour
{
    public Transform handleBase; // 핸들의 고정 위치
    private bool isGrabbed = false;
    private Transform grabbingHand;

    private Vector3 initialHandPosition;
    private float initialHandleRotation;

    void Update()
    {
        if (isGrabbed)
        {
            // 손의 상대적 움직임을 계산
            Vector3 handOffset = grabbingHand.position - initialHandPosition;

            // Z축 기준으로 회전 계산
            float rotationDelta = Vector3.Dot(handOffset, handleBase.right) * 360f;

            // 핸들 회전 적용
            transform.localEulerAngles = new Vector3(0, 0, initialHandleRotation + rotationDelta);
        }
    }

    public void OnGrab(Transform hand)
    {
        isGrabbed = true;
        grabbingHand = hand;

        // 초기 위치 및 회전값 저장
        initialHandPosition = hand.position;
        initialHandleRotation = transform.localEulerAngles.z;
    }

    public void OnRelease()
    {
        isGrabbed = false;
        grabbingHand = null;
    }
}