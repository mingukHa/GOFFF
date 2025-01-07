using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve 관련 스크립트
public class Valve : MonoBehaviourPun, IPunObservable
{
    public BoxCollider cylinderCollider;
    public Transform cylinderAttachPoint;  // 밸브가 실린더에 붙을 위치 변수

    public GameObject knobValve;
    public GameObject grabValve;

    public Transform bridgePlus;  // 회전하는 다리 변수1
    public Transform bridgeMinous;  // 회전하는 다리 변수2

    public GrabValve grabScript;
    public XRGrabInteractable grabInteractable;  // XR Grab Interactable
    public Transform grabTr;

    private bool isAttached = false;  // 밸브가 실린더에 붙었는지 판별하는 변수
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
        //    // 현재 로컬 플레이어가 물체를 잡고 있는 경우에만 위치 업데이트
        //    photonView.RPC("RPCUpdatePosition", RpcTarget.Others, grabValve.transform.localPosition, grabValve.transform.localRotation);
        //}
        // Knob 밸브를 잡지 않고 있으면 자동으로 돌아가면서 
        // Valve 값이 0이 됨
        if (!isGrabbed && knobValve.activeSelf)
        {
            float duration = valveDuration * knob.value;
            knob.value = Mathf.SmoothDamp(knob.value, 0f, ref valveVelocity, duration);

        }

        bridgePlus.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(90f, 0f, knob.value), 0f, 0f));
        bridgeMinous.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(-90f, 0f, knob.value), 0f, 0f));
        
    }

    // 실린더에 밸브를 붙이는 메서드
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

    // 실린더에서 밸브를 떼는 메서드
    public void DetachFromCylinder()
    {
        isAttached = false;  // 밸브가 실린더에서 떨어졌다고 표시


        Delay = ColliderDelay(2f); // 코루틴이 계속 활성화되지 않도록 새로운 코루틴을 할당
        StartCoroutine(Delay);

        knobValve.SetActive(false);
        grabValve.transform.position = cylinderAttachPoint.position;  // 밸브의 위치를 AttachPoint 위치로 설정
        grabValve.transform.rotation = cylinderAttachPoint.rotation;  // 밸브의 회전을 AttachPoint 회전으로 설정
        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;

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

    public void OnSelectValve()
    {
        Debug.Log("Knob 밸브를 잡음");
        //isGrabed = true;
        photonView.RPC("RPCValveGrab", RpcTarget.All, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob 밸브를 놓음");
        //isGrabed = false;
        photonView.RPC("RPCValveGrab", RpcTarget.All, false);
    }

    [PunRPC]
    private void RPCValveGrab(bool grabbed)
    {
        isGrabbed = grabbed;

        if (grabbed)
        {
            // 물체를 잡은 플레이어가 주인이 됨
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }

    //[PunRPC]
    //private void RPCUpdatePosition(Vector3 position, Quaternion rotation)
    //{
    //    // 네트워크에서 받은 위치와 회전값 적용
    //    if (!photonView.IsMine) // 다른 클라이언트에서만 적용
    //    {
    //        grabValve.transform.position = position;
    //        grabValve.transform.rotation = rotation;
    //    }
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // stream - 데이터를 주고 받는 통로 
        // 내가 데이터를 보내는 중이라면
        if (stream.IsWriting && isGrabbed)
        {
            // 이 방안에 있는 모든 사용자에게 브로드캐스트 
            // - 내 포지션 값을 보내보자
            stream.SendNext(grabValve.transform.position);
            stream.SendNext(grabValve.transform.rotation);
        }
        // 내가 데이터를 받는 중이라면 
        else if (isGrabbed)
        {
            // 순서대로 보내면 순서대로 들어옴. 근데 타입캐스팅 해주어야 함
            grabValve.transform.position = (Vector3)stream.ReceiveNext();
            grabValve.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}