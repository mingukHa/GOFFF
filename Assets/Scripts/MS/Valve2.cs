using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// Valve 관련 스크립트
public class Valve2 : MonoBehaviourPun
{
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

    private GameObject currentCylinder;

    private IEnumerator Delay;

    private PhotonTransformView photonTransformView;

    public bool IsAttached { get { return isAttached; } }

    private void Start()
    {

        knob = knobValve.GetComponent<XRKnob>();

        Delay = ColliderDelay(2f);

        grabScript.grabValve2Trigger = grabValve2TriggerHandle;
    }

    private void Update()
    {
        // 마스터 클라이언트에서 계산
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isGrabbed && knobValve.activeSelf)
            {
                float duration = valveDuration * knob.value;
                knob.value = Mathf.SmoothDamp(knob.value, 0f, ref valveVelocity, duration);

                // RPC로 knob.value 동기화
                photonView.RPC("RPCSyncKnobValue", RpcTarget.Others, knob.value);
            }

            if (isAttached)
            {
                float plusRotation = Mathf.Lerp(90f, 0f, knob.value);
                float minusRotation = Mathf.Lerp(-90f, 0f, knob.value);

                bridgePlus.rotation = Quaternion.Euler(new Vector3(plusRotation, 0f, 0f));
                bridgeMinous.rotation = Quaternion.Euler(new Vector3(minusRotation, 0f, 0f));

                // RPC로 회전 값 동기화
                photonView.RPC("RPCSyncBridgeRotation", RpcTarget.Others, plusRotation, minusRotation);
            }
        }
    }

    // knob.value 동기화
    [PunRPC]
    private void RPCSyncKnobValue(float syncedValue)
    {
        knob.value = syncedValue;
    }

    // 다리 회전 값 동기화
    [PunRPC]
    private void RPCSyncBridgeRotation(float plusRotation, float minusRotation)
    {
        bridgePlus.rotation = Quaternion.Euler(new Vector3(plusRotation, 0f, 0f));
        bridgeMinous.rotation = Quaternion.Euler(new Vector3(minusRotation, 0f, 0f));
    }

    // 실린더에 밸브를 붙이는 메서드
    private void AttachToCylinder(GameObject cylinder, GameObject grabValve)
    {
        if (isAttached) return;
        Debug.Log("밸브가 어태치 되었습니다.");
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

    // 실린더에서 밸브를 떼는 메서드
    public void DetachFromCylinder()
    {
        if (!isAttached)
        {
            Debug.Log("밸브가 달려있지 않습니다.");
            return;
        }
        isAttached = false;  // 밸브가 실린더에서 떨어졌다고 표시


        Debug.Log("Detach 실린더 함수가 실행됨");
        Delay = ColliderDelay(2f); // 코루틴이 계속 활성화되지 않도록 새로운 코루틴을 할당
        StartCoroutine(Delay);

        knobValve.SetActive(false);
        //photonView.RPC("RPCknobValvefalse", RpcTarget.Others);
        Debug.Log("Detach 크놉 밸브가 꺼짐");

        PhotonTransformView transformView = grabValve.GetComponent<PhotonTransformView>();
        transformView.enabled = false;
        grabValve.transform.position = currentCylinder.transform.position + new Vector3(0.013f, 0f, 0f);  // 밸브의 위치를 AttachPoint 위치로 설정
        grabValve.transform.rotation = currentCylinder.transform.rotation;  // 밸브의 회전을 AttachPoint 회전으로 설정
        transformView.enabled = true;

        Rigidbody grabValveRb = grabValve.GetComponent<Rigidbody>();
        grabValveRb.linearVelocity = Vector3.zero;
        grabValveRb.angularVelocity = Vector3.zero;
    }

    private IEnumerator ColliderDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentCylinder.GetComponent<BoxCollider>().enabled = true;


        Debug.Log("실린더 콜라이더 활성화");
    }

    private void grabValve2TriggerHandle(GameObject grabValve, Collider other)
    {
        if (other.CompareTag("Cylinder") && !isAttached)
        {
            Debug.Log("게임 오브젝트 : " + grabValve.name + "콜라이더" + other.name);
            AttachToCylinder(other.gameObject, grabValve);
            photonView.RPC("RPCAttachToCylinder2", RpcTarget.Others, other.gameObject, grabValve);
        }
    }

    [PunRPC]
    private void RPCAttachToCylinder2(Collider other, GameObject garbValve)
    {
        AttachToCylinder(other.gameObject, grabValve);
    }

    public void OnSelectValve()
    {
        Debug.Log("Knob 밸브를 잡음");
        isGrabbed = true;
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        photonView.RPC("RPCValveGrab", RpcTarget.Others, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob 밸브를 놓음");
        isGrabbed = false;
        photonView.RPC("RPCValveGrab", RpcTarget.Others, false);
    }

    [PunRPC]
    private void RPCValveGrab(bool grabbed)
    {
        isGrabbed = grabbed;
    }

    [PunRPC]
    private void RPCknobValvefalse()
    {
        knobValve.SetActive(false);
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

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // stream - 데이터를 주고 받는 통로 
    //    // 내가 데이터를 보내는 중이라면
    //    if (stream.IsWriting && isGrabbed)
    //    {
    //        // 이 방안에 있는 모든 사용자에게 브로드캐스트 
    //        // - 내 포지션 값을 보내보자
    //        stream.SendNext(grabValve.transform.position);
    //        stream.SendNext(grabValve.transform.rotation);
    //    }
    //    // 내가 데이터를 받는 중이라면 
    //    else if (isGrabbed)
    //    {
    //        // 순서대로 보내면 순서대로 들어옴. 근데 타입캐스팅 해주어야 함
    //        grabValve.transform.position = (Vector3)stream.ReceiveNext();
    //        grabValve.transform.rotation = (Quaternion)stream.ReceiveNext();
    //    }
    //}

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }
}