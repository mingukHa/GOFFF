using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew2 : MonoBehaviourPun
{
    public XRKnob xrKnob;
    private bool isSyncing = false;
    private float targetValue;
    private float currentValue;
    private float valueVelocity;
    [SerializeField] private float smoothTime = 0.1f;

    private void Start()
    {
        currentValue = xrKnob.value;
        targetValue = currentValue;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // �ε巯�� ���� ����
            currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref valueVelocity, smoothTime);
            if (!isSyncing)
            {
                isSyncing = true;
                xrKnob.SetValue(currentValue);
                isSyncing = false;
            }
        }
    }

    public void HandleSyncKnobValue()
    {
        if (isSyncing) return;
        if (photonView.IsMine)
        {
            //photonView.RPC("SyncKnobValue", RpcTarget.Others, xrKnob.value);
            photonView.RPC("SyncKnobValue2", RpcTarget.All, xrKnob.value);
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
        if (!photonView.IsMine)
        {
            //isSyncing = true;
            xrKnob.SetValue(value);
            //isSyncing = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(xrKnob.value);
        }
        else
        {
            targetValue = (float)stream.ReceiveNext();
        }
    }
    //[PunRPC]
    //void SyncKnobRotation(float angle)
    //{
    //    isSyncing = true;
    //    xrKnob.SetKnobRotation(angle);
    //    isSyncing = false;
    //}
}


//using Photon.Pun;
//using UnityEngine;
//using UnityEngine.XR.Content.Interaction;

//public class SyncKnobNew2 : MonoBehaviourPun
//{
//    public XRKnob xrKnob;
//    private bool isSyncing = false;

//    //private void OnEnable()
//    //{
//    //    // XRKnob���� ���� ����� ������ HandleSyncKnobValue ȣ��
//    //    xrKnob.onValueChange.AddListener(HandleSyncKnobValue);
//    //}

//    //private void OnDisable()
//    //{
//    //    // �̺�Ʈ ���� ����
//    //    xrKnob.onValueChange.RemoveListener(HandleSyncKnobValue);
//    //}

//    public void HandleSyncKnobValue()
//    {
//        if (isSyncing) return;
//        if (photonView.IsMine)
//        {
//            photonView.RPC("SyncKnobValue", RpcTarget.Others, xrKnob.value);
//        }
//    }

//    //public void HandleSyncKnobRotation(float angle)
//    //{
//    //    if (isSyncing) return;
//    //    photonView.RPC("SyncKnobRotation", RpcTarget.Others, angle);
//    //}

//    [PunRPC]
//    void SyncKnobValueNew2(float value)
//    {
//        isSyncing = true;
//        xrKnob.SetValue(value);
//        isSyncing = false;
//    }

//    //[PunRPC]
//    //void SyncKnobRotation(float angle)
//    //{
//    //    isSyncing = true;
//    //    xrKnob.SetKnobRotation(angle);
//    //    isSyncing = false;
//    //}
//}
