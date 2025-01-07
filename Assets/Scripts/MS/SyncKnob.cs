using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Content.Interaction;

public class SyncKnob : MonoBehaviourPun
{
    public XRKnob xrKnob;

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
        photonView.RPC("SyncKnobValue", RpcTarget.Others, value);
    }

    public void HandleSyncKnobRotation(float angle)
    {
        photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    }

    [PunRPC]
    void SyncKnobValue(float value)
    {
        xrKnob.SetValue(value);
    }

    [PunRPC]
    void SyncKnobRotation(float angle)
    {
        xrKnob.SetKnobRotation(angle);
    }
}
