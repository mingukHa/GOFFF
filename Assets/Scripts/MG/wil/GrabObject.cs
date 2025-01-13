using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabObject : XRGrabInteractable
{
    [SerializeField]
    private float grabSpeed = 50f; // ��ü�� ������ �������� �ӵ�

    private Rigidbody objectRigidbody; // ��ü�� Rigidbody
    private Transform grabTarget; // ��(Interactor)�� Transform

    protected override void Awake()
    {
        base.Awake();
        objectRigidbody = GetComponent<Rigidbody>();

        // Rigidbody ����: IsKinematic Ȱ��ȭ (�ʼ�)
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Interactor�� Transform�� GrabTarget���� ����
        grabTarget = args.interactorObject.transform;

        // ��ü�� ������ ������� �ڷ�ƾ ����
        StartCoroutine(GrabToHand());
    }

    private IEnumerator GrabToHand()
    {
        if (objectRigidbody == null)
        {
            Debug.LogError("Rigidbody�� �����ϴ�! ��ü�� Rigidbody�� �߰��ϼ���.");
            yield break;
        }

        // �� ��ġ�� �̵�
        while (Vector3.Distance(transform.position, grabTarget.position) > 0.01f)
        {
            Vector3 direction = (grabTarget.position - transform.position).normalized;
            float step = grabSpeed * Time.deltaTime;

            // Rigidbody�� ����Ͽ� �̵�
            objectRigidbody.MovePosition(transform.position + direction * step);
            yield return null;
        }

        // ��Ȯ�� �� ��ġ�� ����
        objectRigidbody.MovePosition(grabTarget.position);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // GrabTarget �ʱ�ȭ
        grabTarget = null;
    }
}
