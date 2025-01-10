using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Content.Interaction;

public class SyncKnob : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve valve;
    private bool isSyncing = false;

    public void OnSelectValve()
    {
        Debug.Log("Knob 밸브를 잡음");
        valve.IsGrabbed = true;
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        photonView.RPC("RPCValveGrab", RpcTarget.Others, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob 밸브를 놓음");
        valve.IsGrabbed = false;
        photonView.RPC("RPCValveGrab", RpcTarget.Others, false);
    }

    [PunRPC]
    private void RPCValveGrab(bool grabbed)
    {
        valve.IsGrabbed = grabbed;
    }

    //private void OnEnable()
    //{
    //    // XRKnob에서 값이 변경될 때마다 HandleSyncKnobValue 호출
    //    xrKnob.onValueChange.AddListener(HandleSyncKnobValue);
    //}

    //private void OnDisable()
    //{
    //    // 이벤트 구독 해제
    //    xrKnob.onValueChange.RemoveListener(HandleSyncKnobValue);
    //}

    //public void HandleSyncKnobValue()
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobValue", RpcTarget.Others, xrKnob.value);
    //}

    //public void HandleSyncKnobRotation(float angle)
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    //}

    //[PunRPC]
    //void SyncKnobValue(float syncvalue)
    //{
    //    isSyncing = true;
    //    xrKnob.SetValue(syncvalue);
    //    isSyncing = false;
    //}

    //[PunRPC]
    //void SyncKnobRotation(float angle)
    //{
    //    isSyncing = true;
    //    xrKnob.SetKnobRotation(angle);
    //    isSyncing = false;
    //}


}
