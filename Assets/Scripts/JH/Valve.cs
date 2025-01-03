using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve 관련 스크립트
public class Valve : MonoBehaviour
{
    public BoxCollider cylinderCollider;
    public Transform cylinderAttachPoint;  // 밸브가 실린더에 붙을 위치 변수

    public GameObject knobValve;
    public GameObject grabValve;

    public GameObject bridge1;  // 회전하는 다리 변수1
    public GameObject bridge2;  // 회전하는 다리 변수2

    public KnobValve knobScript;
    public GrabValve grabScript;
    public XRGrabInteractable grabInteractable;  // XR Grab Interactable
    public Transform grabTr;

    private Rigidbody valveRigidbody;  // 밸브의 Rigidbody를 변수
    private bool isAttached = false;  // 밸브가 실린더에 붙었는지 판별하는 변수
    private bool isRotating = false;  // 밸브가 회전 판별하는 변수
    private float rotationSpeed = 10f;  // 밸브 회전 속도
    private float currentRotationZ = 0f;  // 밸브의 현재 z축 회전값

    private XRKnob knob;
    //private Transform attachTransform;  // 손 위치 추적을 위한 변수

    private IEnumerator Delay;

    public bool IsAttached { get { return isAttached; } }

    void Start()
    {
        // Rigidbody 컴포넌트를 가져와서 valveRigidbody 변수에 저장
        valveRigidbody = grabValve.GetComponent<Rigidbody>();

        knob = GetComponent<XRKnob>();

        Delay = ColliderDelay(1f);

        grabScript.grabValveTrigger = grabValveTriggerHandle;
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

    //// 다른 Collider가 이 밸브와 충돌했을 때 호출되는 메서드
    //private void OnTriggerEnter(Collider other)
    //{
    //    // 충돌한 오브젝트가 "Cylinder" 태그를 가지고 있고, 밸브가 아직 실린더에 붙지 않았다면
    //    if (other.CompareTag("Cylinder") && !isAttached)
    //    {
    //        Debug.Log("실린더에 충돌함");
    //        AttachToCylinder(other.gameObject);  // 실린더에 밸브를 붙이는 메서드 호출
    //    }
    //}

    // 실린더에 밸브를 붙이는 메서드
    private void AttachToCylinder(GameObject cylinder, GameObject grabValve)
    {
        if (isAttached) return;

        if (Delay != null)
        {
            StopCoroutine(Delay);
        }

        //if (cylinderAttachPoint != null)
        //{
        //    grabValve.transform.position = cylinderAttachPoint.position;  // 밸브의 위치를 AttachPoint 위치로 설정
        //    grabValve.transform.rotation = cylinderAttachPoint.rotation;  // 밸브의 회전을 AttachPoint 회전으로 설정
        //    isAttached = true;  // 밸브가 실린더에 붙어 있다고 표시
        //}

        isAttached = true;
        grabValve.transform.position = grabTr.position;
        knobValve.SetActive(true);
    }

    // 실린더에서 밸브를 떼는 메서드
    public void DetachFromCylinder()
    {
        isAttached = false;  // 밸브가 실린더에서 떨어졌다고 표시


        Delay = ColliderDelay(2f); // 코루틴이 계속 활성화되지 않도록 새로운 코루틴을 할당
        StartCoroutine(Delay);

        knobValve.SetActive(false);
        grabValve.transform.position = cylinderAttachPoint.position;  // 밸브의 위치를 AttachPoint 위치로 설정
        grabValve.transform.rotation = cylinderAttachPoint.rotation;  // 밸브의 회전을 AttachPoint 회전으로 설정

        //// Interaction Manager 및 Collider 상태 확인 및 재설정
        //grabInteractable.interactionManager = FindFirstObjectByType<XRInteractionManager>();
        //grabInteractable.attachTransform = transform; // 필요시 attachTransform 재설정
        //grabInteractable.GetComponent<Collider>().enabled = true;


    }

    private IEnumerator ColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cylinderCollider.enabled = true;

        Debug.Log("실린더 콜라이더 활성화");
    }

    private void grabValveTriggerHandle(GameObject grabValve, Collider other)
    {
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            Debug.Log("게임 오브젝트 : " + grabValve.name + "콜라이더" + other.name);
            AttachToCylinder(other.gameObject, grabValve);
        }
    }

}