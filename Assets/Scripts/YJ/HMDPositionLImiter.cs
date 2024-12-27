using UnityEngine;
using UnityEngine.XR;

public class HMDPositionLimiter : MonoBehaviour
{
    public float maxDistance = 0.1f; // 이동 허용 범위 (미터)
    private Vector3 initialPosition; // 초기 위치
    public Transform hmdTransform; // HMD의 Transform

    void Start()
    {
        // 초기 위치 설정 (플레이어가 게임을 시작한 위치)
        hmdTransform = Camera.main.transform; // HMD 카메라의 Transform
        initialPosition = hmdTransform.localPosition; // 로컬 위치를 기준으로 계산
    }

    void Update()
    {
        // 현재 HMD 위치 가져오기
        Vector3 currentPosition = hmdTransform.localPosition;

        // 초기 위치와의 거리 계산
        Vector3 offset = currentPosition - initialPosition;

        // 거리가 제한 범위를 초과하면 제한
        if (offset.magnitude > maxDistance)
        {
            // 방향을 유지한 채로 제한 범위 내로 이동
            offset = offset.normalized * maxDistance;
            hmdTransform.localPosition = initialPosition + offset;
        }
    }
}
