using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve ���� ��ũ��Ʈ
public class Valve : MonoBehaviour
{
    public BoxCollider cylinderCollider;
    public Transform cylinderAttachPoint;  // ��갡 �Ǹ����� ���� ��ġ ����

    public GameObject knobValve;
    public GameObject grabValve;

    public GameObject bridge1;  // ȸ���ϴ� �ٸ� ����1
    public GameObject bridge2;  // ȸ���ϴ� �ٸ� ����2

    public KnobValve knobScript;
    public GrabValve grabScript;
    public XRGrabInteractable grabInteractable;  // XR Grab Interactable
    public Transform grabTr;

    private Rigidbody valveRigidbody;  // ����� Rigidbody�� ����
    private bool isAttached = false;  // ��갡 �Ǹ����� �پ����� �Ǻ��ϴ� ����
    private bool isRotating = false;  // ��갡 ȸ�� �Ǻ��ϴ� ����
    private float rotationSpeed = 10f;  // ��� ȸ�� �ӵ�
    private float currentRotationZ = 0f;  // ����� ���� z�� ȸ����

    private XRKnob knob;
    //private Transform attachTransform;  // �� ��ġ ������ ���� ����

    private IEnumerator Delay;

    public bool IsAttached { get { return isAttached; } }

    void Start()
    {
        // Rigidbody ������Ʈ�� �����ͼ� valveRigidbody ������ ����
        valveRigidbody = grabValve.GetComponent<Rigidbody>();

        knob = GetComponent<XRKnob>();

        Delay = ColliderDelay(1f);

        grabScript.grabValveTrigger = grabValveTriggerHandle;
    }

    void Update()
    {
        // isRotating�� true�̰�, currentRotationZ�� 360�� �̸��� ���� ȸ�� ������Ʈ
        if (isRotating && currentRotationZ < 360f)
        {
            currentRotationZ += rotationSpeed * Time.deltaTime;  // ȸ�� ���� ������Ʈ (Time.deltaTime�� ���ؼ� ������ ���������� ȸ��)
            if (currentRotationZ >= 360f) currentRotationZ = 360f;  // ȸ�� ���� 360���� ���� �ʵ��� ����

            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);  // z�� ȸ������ �����Ͽ� ����� ȸ�� ������Ʈ
            if (bridge1 != null)  // bridge1�� null�� �ƴϸ�
            {
                // currentRotationZ�� ����Ͽ� bridge1�� ȸ�� ���� ����ϰ� ����
                float bridge1RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge1.transform.rotation = Quaternion.Euler(bridge1RotationX, 0, 0);  // ���� ȸ������ ����
            }

            if (bridge2 != null)  // bridge2�� null�� �ƴϸ�
            {
                // currentRotationZ�� ����Ͽ� bridge2�� ȸ�� ���� ����ϰ� ����
                float bridge2RotationX = Mathf.Lerp(-90f, 0f, currentRotationZ / 360f);
                bridge2.transform.rotation = Quaternion.Euler(bridge2RotationX, 0, 0);  // ���� ȸ������ ����
            }
        }
    }

    //// �ٸ� Collider�� �� ���� �浹���� �� ȣ��Ǵ� �޼���
    //private void OnTriggerEnter(Collider other)
    //{
    //    // �浹�� ������Ʈ�� "Cylinder" �±׸� ������ �ְ�, ��갡 ���� �Ǹ����� ���� �ʾҴٸ�
    //    if (other.CompareTag("Cylinder") && !isAttached)
    //    {
    //        Debug.Log("�Ǹ����� �浹��");
    //        AttachToCylinder(other.gameObject);  // �Ǹ����� ��긦 ���̴� �޼��� ȣ��
    //    }
    //}

    // �Ǹ����� ��긦 ���̴� �޼���
    private void AttachToCylinder(GameObject cylinder, GameObject grabValve)
    {
        if (isAttached) return;

        if (Delay != null)
        {
            StopCoroutine(Delay);
        }

        //if (cylinderAttachPoint != null)
        //{
        //    grabValve.transform.position = cylinderAttachPoint.position;  // ����� ��ġ�� AttachPoint ��ġ�� ����
        //    grabValve.transform.rotation = cylinderAttachPoint.rotation;  // ����� ȸ���� AttachPoint ȸ������ ����
        //    isAttached = true;  // ��갡 �Ǹ����� �پ� �ִٰ� ǥ��
        //}

        isAttached = true;
        grabValve.transform.position = grabTr.position;
        knobValve.SetActive(true);
    }

    // �Ǹ������� ��긦 ���� �޼���
    public void DetachFromCylinder()
    {
        isAttached = false;  // ��갡 �Ǹ������� �������ٰ� ǥ��


        Delay = ColliderDelay(2f); // �ڷ�ƾ�� ��� Ȱ��ȭ���� �ʵ��� ���ο� �ڷ�ƾ�� �Ҵ�
        StartCoroutine(Delay);

        knobValve.SetActive(false);
        grabValve.transform.position = cylinderAttachPoint.position;  // ����� ��ġ�� AttachPoint ��ġ�� ����
        grabValve.transform.rotation = cylinderAttachPoint.rotation;  // ����� ȸ���� AttachPoint ȸ������ ����

        //// Interaction Manager �� Collider ���� Ȯ�� �� �缳��
        //grabInteractable.interactionManager = FindFirstObjectByType<XRInteractionManager>();
        //grabInteractable.attachTransform = transform; // �ʿ�� attachTransform �缳��
        //grabInteractable.GetComponent<Collider>().enabled = true;


    }

    private IEnumerator ColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cylinderCollider.enabled = true;

        Debug.Log("�Ǹ��� �ݶ��̴� Ȱ��ȭ");
    }

    private void grabValveTriggerHandle(GameObject grabValve, Collider other)
    {
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            Debug.Log("���� ������Ʈ : " + grabValve.name + "�ݶ��̴�" + other.name);
            AttachToCylinder(other.gameObject, grabValve);
        }
    }

}