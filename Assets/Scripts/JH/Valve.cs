using UnityEngine;

// Valve 관련 스크립트
public class Valve : MonoBehaviour
{
    public Transform cylinderAttachPoint;  // 밸브가 실린더에 붙을 위치 변수
    private Rigidbody valveRigidbody;  // 밸브의 Rigidbody를 변수
    private bool isAttached = false;  // 밸브가 실린더에 붙었는지 판별하는 변수
    private bool isRotating = false;  // 밸브가 회전 판별하는 변수
    private float rotationSpeed = 10f;  // 밸브 회전 속도
    private float currentRotationZ = 0f;  // 밸브의 현재 z축 회전값

    public GameObject bridge1;  // 회전하는 다리 변수1
    public GameObject bridge2;  // 회전하는 다리 변수2

    void Start()
    {
        // Rigidbody 컴포넌트를 가져와서 valveRigidbody 변수에 저장
        valveRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // isRotating이 true이고, currentRotationZ가 360도 미만일 때만 회전 업데이트
        if (isRotating && currentRotationZ < 360f)
        {
            currentRotationZ += rotationSpeed * Time.deltaTime;  // 회전 값을 업데이트 (Time.deltaTime을 곱해서 프레임 독립적으로 회전)
            if (currentRotationZ >= 360f) currentRotationZ = 360f;  // 회전 값이 360도를 넘지 않도록 제한

            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);  // z축 회전값을 적용하여 밸브의 회전 업데이트
            if (bridge1 != null)  // bridge1이 null이 아니면
            {
                // currentRotationZ에 비례하여 bridge1의 회전 값을 계산하고 적용
                float bridge1RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge1.transform.rotation = Quaternion.Euler(bridge1RotationX, 0, 0);  // 계산된 회전값을 적용
            }

            if (bridge2 != null)  // bridge2가 null이 아니면
            {
                // currentRotationZ에 비례하여 bridge2의 회전 값을 계산하고 적용
                float bridge2RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge2.transform.rotation = Quaternion.Euler(bridge2RotationX, 0, 0);  // 계산된 회전값을 적용
            }
        }
    }

    // 다른 Collider가 이 밸브와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Cylinder" 태그를 가지고 있고, 밸브가 아직 실린더에 붙지 않았다면
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            AttachToCylinder(other.gameObject);  // 실린더에 밸브를 붙이는 메서드 호출
        }
    }

    // 다른 Collider와 충돌이 끝났을 때 호출되는 메서드
    private void OnTriggerExit(Collider other)
    {
        // 충돌한 오브젝트가 "Cylinder" 태그를 가지고 있고, 밸브가 실린더에 붙어 있다면
        if (other.CompareTag("Cylinder") && isAttached)
        {
            DetachFromCylinder();  // 실린더에서 밸브를 떼는 메서드 호출
        }
    }

    // 실린더에 밸브를 붙이는 메서드
    private void AttachToCylinder(GameObject cylinder)
    {
        if (isAttached) return;  // 이미 밸브가 실린더에 붙어 있다면 아무 작업도 하지 않음

        // Rigidbody 비활성화 (중력 영향을 받지 않도록)
        valveRigidbody.isKinematic = true;
        valveRigidbody.useGravity = false;

        // 실린더의 AttachPoint 위치를 찾아서, 그 위치와 회전 값으로 밸브를 설정
        Transform attachPoint = cylinder.transform.Find("AttachPoint");
        if (attachPoint != null)
        {
            transform.position = attachPoint.position;  // 밸브의 위치를 AttachPoint 위치로 설정
            transform.rotation = attachPoint.rotation;  // 밸브의 회전을 AttachPoint 회전으로 설정
            isAttached = true;  // 밸브가 실린더에 붙어 있다고 표시
        }
    }

    // 실린더에서 밸브를 떼는 메서드
    private void DetachFromCylinder()
    {
        if (!isAttached) return;  // 밸브가 실린더에 붙어 있지 않다면 아무 작업도 하지 않음

        // Rigidbody 원래 상태로 복원 (중력 영향을 받도록 설정)
        valveRigidbody.isKinematic = false;
        valveRigidbody.useGravity = true;
        isAttached = false;  // 밸브가 실린더에서 떨어졌다고 표시
    }

    // 외부에서 회전을 시작하는 메서드
    public void StartRotation()
    {
        isRotating = true;  // 회전 시작
    }

    // 외부에서 회전을 멈추는 메서드
    public void StopRotation()
    {
        isRotating = false;  // 회전 멈춤
    }
}