using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ValveVelocity : MonoBehaviour
{
    public XRBaseInteractor leftController; // 왼쪽 컨트롤러
    public XRBaseInteractor rightController; // 오른쪽 컨트롤러
    public Transform handle; // 회전할 핸들 오브젝트

    private Vector3 previousLeftPosition; // 이전 왼쪽 컨트롤러 위치
    private Vector3 previousRightPosition; // 이전 오른쪽 컨트롤러 위치

    void Start()
    {
        // 컨트롤러 초기 위치 저장
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;
    }

    void Update()
    {
        // 두 컨트롤러의 위치 차이를 계산
        Vector3 leftDelta = leftController.transform.position - previousLeftPosition;
        Vector3 rightDelta = rightController.transform.position - previousRightPosition;

        // 두 손의 이동 차이를 합산하여 회전 값 계산
        float rotationAmount = (leftDelta.x + rightDelta.x) * 10f; // 회전 속도 조정

        // 핸들을 회전시킴
        handle.Rotate(Vector3.up, rotationAmount);

        // 이전 위치를 업데이트
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;
    }
}