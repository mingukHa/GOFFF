using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve ���� ��ũ��Ʈ
public class Valve : MonoBehaviourPun
{
    public GameObject knobValve;
    public GameObject grabValve;

    public Transform bridgePlus;  // ȸ���ϴ� �ٸ� ����1
    public Transform bridgeMinous;  // ȸ���ϴ� �ٸ� ����2

    public GrabValve grabScript;
    public XRGrabInteractable grabInteractable;  // XR Grab Interactable
    public Transform grabTr;

    private bool isAttached = false;  // ��갡 �Ǹ����� �پ����� �Ǻ��ϴ� ����
    private bool isGrabbed = false;

    private XRKnob knob;
    private float valveDuration = 3f;

    private float valveVelocity = 0f;

    private GameObject currentCylinder;

    private IEnumerator Delay;

    private PhotonTransformView photonTransformView;

    public bool IsAttached { get { return isAttached; } }
    public bool IsGrabbed { get { return isGrabbed; } }

    private void Start()
    {

        knob = knobValve.GetComponent<XRKnob>();

        Delay = ColliderDelay(2f);

        grabScript.grabValveTrigger = grabValveTriggerHandle;
    }

    private void Update()
    {
        Debug.Log("���� isGrabbed�� ���� : " + IsGrabbed);
        // ��� Ŭ���̾�Ʈ���� ���
        if (!isGrabbed && knobValve.activeSelf)
        {
            float duration = valveDuration * knob.value;
            knob.value = Mathf.SmoothDamp(knob.value, 0f, ref valveVelocity, duration);
        }

        if (isAttached)
        {
            float plusRotation = Mathf.Lerp(90f, 0f, knob.value);
            float minusRotation = Mathf.Lerp(-90f, 0f, knob.value);

            bridgePlus.rotation = Quaternion.Euler(new Vector3(plusRotation, 0f, 0f));
            bridgeMinous.rotation = Quaternion.Euler(new Vector3(minusRotation, 0f, 0f));
        }
    }

    // knob.value ����ȭ
    [PunRPC]
    private void RPCSyncKnobValue(float syncedValue)
    {
        knob.value = syncedValue;
    }

    // �ٸ� ȸ�� �� ����ȭ
    [PunRPC]
    private void RPCSyncBridgeRotation(float plusRotation, float minusRotation)
    {
        bridgePlus.rotation = Quaternion.Euler(new Vector3(plusRotation, 0f, 0f));
        bridgeMinous.rotation = Quaternion.Euler(new Vector3(minusRotation, 0f, 0f));
    }
    // �Ǹ����� ��긦 ���̴� �޼���
    private void AttachToCylinder(GameObject cylinder, GameObject grabValve)
    {
        if (isAttached) return;
        Debug.Log("��갡 ����ġ �Ǿ����ϴ�.");
        if (Delay != null)
        {
            StopCoroutine(Delay);
        }

        currentCylinder = cylinder;
        currentCylinder.GetComponent<BoxCollider>().enabled = false;
        //cylinder.SetActive(false);

        isAttached = true;
        PhotonTransformView transformView = grabValve.GetComponent<PhotonTransformView>();
        transformView.enabled = false;
        grabValve.transform.position = grabTr.position;
        transformView.enabled = true;
        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;
        knobValve.SetActive(true);
    }

    // �Ǹ������� ��긦 ���� �޼���
    public void DetachFromCylinder()
    {
        if (!isAttached)
        {
            Debug.Log("��갡 �޷����� �ʽ��ϴ�.");
            return;
        }
        isAttached = false;  // ��갡 �Ǹ������� �������ٰ� ǥ��


        Debug.Log("Detach �Ǹ��� �Լ��� �����");
        Delay = ColliderDelay(2f); // �ڷ�ƾ�� ��� Ȱ��ȭ���� �ʵ��� ���ο� �ڷ�ƾ�� �Ҵ�
        StartCoroutine(Delay);

        knobValve.SetActive(false);
        Debug.Log("Detach ũ�� ��갡 ����");

        PhotonTransformView transformView = grabValve.GetComponent<PhotonTransformView>();
        transformView.enabled = false;
        grabValve.transform.position = currentCylinder.transform.position + new Vector3(0.013f, 0f, 0f);  // ����� ��ġ�� AttachPoint ��ġ�� ����
        grabValve.transform.rotation = currentCylinder.transform.rotation;  // ����� ȸ���� AttachPoint ȸ������ ����
        transformView.enabled = true;

        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;
    }

    private IEnumerator ColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentCylinder.GetComponent<BoxCollider>().enabled = true;


        Debug.Log("�Ǹ��� �ݶ��̴� Ȱ��ȭ");
    }

    private void grabValveTriggerHandle(GameObject grabValve, Collider other)
    {
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            AttachToCylinder(other.gameObject, grabValve);
            if(knobValve.activeSelf == false)
            {
                grabValve.transform.position = grabTr.position;
                Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
                grabValveRb.linearVelocity = Vector3.zero;
                grabValveRb.angularVelocity = Vector3.zero;
                knobValve.SetActive(true);
            }
        }
    }

    [PunRPC]
    private void RPCAttachToCylinder(Collider other, GameObject garbValve)
    {
        AttachToCylinder(other.gameObject, grabValve);
    }


    public void OnSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        isGrabbed = true;
        if (!photonView.IsMine)
        {
            Debug.Log("�������� �絵��");
            photonView.RequestOwnership();
        }
        photonView.RPC("RPCValveGrab", RpcTarget.Others, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        isGrabbed = false;
        photonView.RPC("RPCValveGrab", RpcTarget.Others, false);
    }

    [PunRPC]
    private void RPCValveGrab(bool grabbed)
    {
        Debug.Log("RPC Knob ��� ���۵�");
        isGrabbed = grabbed;
        Debug.Log("RPC ������ isGrabbed ���� :" + isGrabbed);
    }

    [PunRPC]
    private void RPCknobValvefalse()
    {
        knobValve.SetActive(false);
    }

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }
}