using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew2 : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve2 valve;
    public GameObject knob;
    private bool isSyncing = false;

    public void HandleSyncKnobValue()
    {
        Debug.Log("Onchanged value�� �����, IsGrabbed �� : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine�� IsGrabbed�� �����");
            photonView.RPC("SyncKnobValueNew2", RpcTarget.Others, xrKnob.value);
        }
    }

    [PunRPC]
    void SyncKnobValueNew2(float value)
    {
        isSyncing = true;
        xrKnob.SetValue(value);
        isSyncing = false;
    }

}
