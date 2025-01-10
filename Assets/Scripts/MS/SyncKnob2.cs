using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Content.Interaction;

public class SyncKnob2 : MonoBehaviourPun
{
    public XRKnob xrKnob;
    private bool isSyncing = false;
    public Valve2 valve;
    public void OnSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        valve.IsGrabbed = true;
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        photonView.RPC("RPCValveGrab2", RpcTarget.Others, true);
    }

    public void OffSelectValve()
    {
        Debug.Log("Knob ��긦 ����");
        valve.IsGrabbed = false;
        photonView.RPC("RPCValveGrab2", RpcTarget.Others, false);
    }

    [PunRPC]
    private void RPCValveGrab2(bool grabbed)
    {
        valve.IsGrabbed = grabbed;
    }

    //private void OnEnable()
    //{
    //    // XRKnob���� ���� ����� ������ HandleSyncKnobValue ȣ��
    //    xrKnob.onValueChange.AddListener(HandleSyncKnobValue);
    //}

    //private void OnDisable()
    //{
    //    // �̺�Ʈ ���� ����
    //    xrKnob.onValueChange.RemoveListener(HandleSyncKnobValue);
    //}

    //public void HandleSyncKnobValue2()
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobValue2", RpcTarget.Others, xrKnob.value);
    //}

    //public void HandleSyncKnobRotation(float angle)
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobRotation2", RpcTarget.Others, angle);
    //}

    //[PunRPC]
    //void SyncKnobValue2(float syncvalue)
    //{
    //    isSyncing = true;
    //    xrKnob.SetValue(syncvalue);
    //    isSyncing = false;
    //}

    //[PunRPC]
    //void SyncKnobRotation2(float angle)
    //{
    //    isSyncing = true;
    //    xrKnob.SetKnobRotation(angle);
    //    isSyncing = false;
    //}
}
