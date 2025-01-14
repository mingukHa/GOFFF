using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve valve;
    public GameObject knob;
    private bool isSyncing = false;
    private bool isAutoRotating = false; // �ڵ� ȸ�� ������ Ȯ���ϴ� �÷���

    private float targetValue; // ��ǥ ��
    private float currentValue; // ���� ��
    public float lerpSpeed = 5f; // ���� �ӵ�

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
        Debug.Log("Onchanged value�� �����, IsGrabbed �� : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine�� IsGrabbed�� �����");
            photonView.RPC("SyncKnobValueNew", RpcTarget.Others, xrKnob.value);
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
