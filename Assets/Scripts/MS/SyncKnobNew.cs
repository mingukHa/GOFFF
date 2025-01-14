using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve valve;
    public GameObject knob;
    private bool isSyncing = false;
    private bool isAutoRotating = false; // 자동 회전 중인지 확인하는 플래그

    private float targetValue; // 목표 값
    private float currentValue; // 현재 값
    public float lerpSpeed = 5f; // 보간 속도

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

    public void HandleSyncKnobValue()
    {
        Debug.Log("Onchanged value가 실행됨, IsGrabbed 값 : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine과 IsGrabbed가 통과됨");
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
