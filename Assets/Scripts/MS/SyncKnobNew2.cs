using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew2 : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve2 valve;
    public GameObject knob;
    private bool isSyncing = false;

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

    void Update()
    {
        // ������ �����Ͽ� ���� ���������� ��ȭ��Ŵ
        if (!isSyncing)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * lerpSpeed);
            xrKnob.SetValue(currentValue); // �ε巴�� �� ����
        }
    }

    public void HandleSyncKnobValue()
    {
        Debug.Log("Onchanged value�� �����, IsGrabbed �� : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine�� IsGrabbed�� �����");
            photonView.RPC("SyncKnobValueNew2", RpcTarget.Others, xrKnob.value);
        }
    }

    //public void HandleSyncKnobRotation(float angle)
    //{
    //    if (isSyncing) return;
    //    photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
    //}

    [PunRPC]
    void SyncKnobValueNew2(float value)
    {
        isSyncing = true;
        xrKnob.SetValue(value);
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
