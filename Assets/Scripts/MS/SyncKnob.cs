using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Content.Interaction;

public class SyncKnob : MonoBehaviourPun
{
    public XRKnob xrKnob;
    private bool isSyncing = false;

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

    public void HandleSyncKnobValue()
    {
        if (isSyncing) return;
        photonView.RPC("SyncKnobValue", RpcTarget.Others);
    }

    //public void HandleSyncKnobRotation(float angle)
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    //}

    [PunRPC]
    void SyncKnobValue()
    {
        isSyncing = true;
        xrKnob.SetValue(xrKnob.value);
        isSyncing = false;
    }

    //[PunRPC]
    //void SyncKnobRotation(float angle)
    //{
    //    isSyncing = true;
    //    xrKnob.SetKnobRotation(angle);
    //    isSyncing = false;
    //}
}
