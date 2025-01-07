using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Content.Interaction;

public class SyncKnob : MonoBehaviourPun
{
    public XRKnob xrKnob;
    private bool isSyncing = false;

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

    public void HandleSyncKnobValue(float value)
    {
        if (isSyncing) return;
        photonView.RPC("SyncKnobValue", RpcTarget.Others, value);
    }

    public void HandleSyncKnobRotation(float angle)
    {
        if (isSyncing) return;
        photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    }

    [PunRPC]
    void SyncKnobValue(float value)
    {
        isSyncing = true;
        xrKnob.SetValue(value);
        isSyncing = false;
    }

    [PunRPC]
    void SyncKnobRotation(float angle)
    {
        isSyncing = true;
        xrKnob.SetKnobRotation(angle);
        isSyncing = false;
    }
}
