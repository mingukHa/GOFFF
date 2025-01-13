using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve valve;
    private bool isSyncing = false;
    private bool isAutoRotating = false; // �ڵ� ȸ�� ������ Ȯ���ϴ� �÷���

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
        if (photonView.IsMine && valve.IsGrabbed)
        {
            photonView.RPC("SyncKnobValue", RpcTarget.Others, xrKnob.value);
        }
    }

    //public void HandleSyncKnobRotation(float angle)
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    //}

    [PunRPC]
    void SyncKnobValueNew(float value)
    {
        isSyncing = true;
        xrKnob.SetValue(value);
        isSyncing = false;
    }

    public void SetAutoRotating(bool autoRotating)
    {
        isAutoRotating = autoRotating;
    }

    //[PunRPC]
    //void SyncKnobRotation(float angle)
    //{
    //    isSyncing = true;
    //    xrKnob.SetKnobRotation(angle);
    //    isSyncing = false;
    //}
}
