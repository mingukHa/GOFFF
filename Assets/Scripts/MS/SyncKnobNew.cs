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
        Debug.Log("Onchanged value°¡ ½ÇÇàµÊ, IsGrabbed °ª : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine°ú IsGrabbed°¡ Åë°úµÊ");
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
