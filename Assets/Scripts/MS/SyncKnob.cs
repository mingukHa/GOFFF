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
