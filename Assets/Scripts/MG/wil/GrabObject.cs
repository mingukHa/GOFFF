using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabObject : XRGrabInteractable
{
    [SerializeField]
    private float grabSpeed = 50f; // 물체가 손으로 끌려오는 속도

    private Rigidbody objectRigidbody; // 물체의 Rigidbody
    private Transform grabTarget; // 손(Interactor)의 Transform

    protected override void Awake()
    {
        base.Awake();
        objectRigidbody = GetComponent<Rigidbody>();

        // Rigidbody 설정: IsKinematic 활성화 (필수)
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Interactor의 Transform을 GrabTarget으로 설정
        grabTarget = args.interactorObject.transform;

        // 물체를 손으로 끌어오는 코루틴 실행
        StartCoroutine(GrabToHand());
    }

    private IEnumerator GrabToHand()
    {
        if (objectRigidbody == null)
        {
            Debug.LogError("Rigidbody가 없습니다! 물체에 Rigidbody를 추가하세요.");
            yield break;
        }

        // 손 위치로 이동
        while (Vector3.Distance(transform.position, grabTarget.position) > 0.01f)
        {
            Vector3 direction = (grabTarget.position - transform.position).normalized;
            float step = grabSpeed * Time.deltaTime;

            // Rigidbody를 사용하여 이동
            objectRigidbody.MovePosition(transform.position + direction * step);
            yield return null;
        }

        // 정확히 손 위치에 고정
        objectRigidbody.MovePosition(grabTarget.position);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // GrabTarget 초기화
        grabTarget = null;
    }
}
