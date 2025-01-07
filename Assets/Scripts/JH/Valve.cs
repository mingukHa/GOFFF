using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve ���� ��ũ��Ʈ
public class Valve : MonoBehaviourPun, IPunObservable
{
    public BoxCollider cylinderCollider;
    public Transform cylinderAttachPoint;  // ��갡 �Ǹ����� ���� ��ġ ����

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
    private float bridgeVelocity = 0f;

    private IEnumerator Delay;

    public bool IsAttached { get { return isAttached; } }

    private void Start()
    {

        knob = knobValve.GetComponent<XRKnob>();

        Delay = ColliderDelay(2f);

        grabScript.grabValveTrigger = grabValveTriggerHandle;
    }

    private void Update()
    {
        //if (isGrabbed && photonView.IsMine)
        //{
        //    // ���� ���� �÷��̾ ��ü�� ��� �ִ� ��쿡�� ��ġ ������Ʈ
        //    photonView.RPC("RPCUpdatePosition", RpcTarget.Others, grabValve.transform.localPosition, grabValve.transform.localRotation);
        //}
        // Knob ��긦 ���� �ʰ� ������ �ڵ����� ���ư��鼭 
        // Valve ���� 0�� ��
        if (!isGrabbed && knobValve.activeSelf)
        {
            float duration = valveDuration * knob.value;
            knob.value = Mathf.SmoothDamp(knob.value, 0f, ref valveVelocity, duration);

        }

        bridgePlus.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(90f, 0f, knob.value), 0f, 0f));
        bridgeMinous.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(-90f, 0f, knob.value), 0f, 0f));
        
    }

    // �Ǹ����� ��긦 ���̴� �޼���
    private void AttachToCylinder(GameObject cylinder, GameObject grabValve)
    {
        if (isAttached) return;

        if (Delay != null)
        {
            StopCoroutine(Delay);
        }

        cylinderCollider.enabled = false;

        isAttached = true;
        grabValve.transform.position = grabTr.position;
        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;
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
        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;

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

    public void OnSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        //isGrabed = true;
        photonView.RPC("RPCValveGrab", RpcTarget.All, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        //isGrabed = false;
        photonView.RPC("RPCValveGrab", RpcTarget.All, false);
    }

    [PunRPC]
    private void RPCValveGrab(bool grabbed)
    {
        isGrabbed = grabbed;

        if (grabbed)
        {
            // ��ü�� ���� �÷��̾ ������ ��
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }

    //[PunRPC]
    //private void RPCUpdatePosition(Vector3 position, Quaternion rotation)
    //{
    //    // ��Ʈ��ũ���� ���� ��ġ�� ȸ���� ����
    //    if (!photonView.IsMine) // �ٸ� Ŭ���̾�Ʈ������ ����
    //    {
    //        grabValve.transform.position = position;
    //        grabValve.transform.rotation = rotation;
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // stream - �����͸� �ְ� �޴� ��� 
        // ���� �����͸� ������ ���̶��
        if (stream.IsWriting && isGrabbed)
        {
            // �� ��ȿ� �ִ� ��� ����ڿ��� ��ε�ĳ��Ʈ 
            // - �� ������ ���� ��������
            stream.SendNext(grabValve.transform.position);
            stream.SendNext(grabValve.transform.rotation);
        }
        // ���� �����͸� �޴� ���̶�� 
        else if (isGrabbed)
        {
            // ������� ������ ������� ����. �ٵ� Ÿ��ĳ���� ���־�� ��
            grabValve.transform.position = (Vector3)stream.ReceiveNext();
            grabValve.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}