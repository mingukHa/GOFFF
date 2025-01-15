using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve valve;
    public GameObject knob;
    private bool isSyncing = false;

    public void HandleSyncKnobValue()
    {
        Debug.Log("Onchanged value�� �����, IsGrabbed �� : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine�� IsGrabbed�� �����");
            photonView.RPC("SyncKnobValueNew", RpcTarget.Others, xrKnob.value);
        }
    }

    [PunRPC]
    void SyncKnobValueNew(float value)
    {
        isSyncing = true;
        xrKnob.SetValue(value);
        isSyncing = false;
    }
}
