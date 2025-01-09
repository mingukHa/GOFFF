using Photon.Pun;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabValve : MonoBehaviourPun
{
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;
    public delegate void grabValve2Delegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValve2Trigger;

    private bool isGrabbed = false;

    private Vector3 currentTR = Vector3.zero;

    private void Start()
    {
        currentTR = transform.position;
    }

    private void Update()
    {
        if(-15f>transform.position.y)
        {
            Rigidbody ValveRD=transform.GetComponent<Rigidbody>();
            ValveRD.angularVelocity = Vector3.zero;
            ValveRD.linearVelocity = Vector3.zero;
            transform.position = currentTR;
            transform.rotation = Quaternion.identity;
        }
    }

    // 다른 Collider가 이 밸브와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbed)
        {
            if (other.name == "CylinderA")
            {
                grabValveTrigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger", RpcTarget.Others ,other);
            }
            else
            {
                grabValve2Trigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger2", RpcTarget.Others, other);
            }
        }
    }

    [PunRPC]
    private void RPCTrigger(Collider other)
    {
        grabValveTrigger?.Invoke(gameObject, other);
    }

    [PunRPC]
    private void RPCTrigger2(Collider other)
    {
        grabValve2Trigger?.Invoke(gameObject, other);
    }

    public void SelectOn()
    {
        Debug.Log("밸브를 잡았습니다.");
        isGrabbed = true;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, true);

    }

    public void SelectOff()
    {
        Debug.Log("밸브를 놓았습니다.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    [PunRPC]
    private void RPCGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
        Rigidbody grabValveRD = transform.GetComponent<Rigidbody>();
        if (grabValveRD != null)
        {
            grabValveRD.isKinematic = grabbed;
        }
    }

}
