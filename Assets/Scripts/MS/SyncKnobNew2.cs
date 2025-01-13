using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SyncKnobNew2 : MonoBehaviourPun
{
    public XRKnob xrKnob;
    public Valve2 valve;
    public GameObject knob;
    private bool isSyncing = false;

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

    void Update()
    {
        // 보간을 적용하여 값을 점진적으로 변화시킴
        if (!isSyncing)
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * lerpSpeed);
            xrKnob.SetValue(currentValue); // 부드럽게 값 설정
        }
    }

    public void HandleSyncKnobValue()
    {
        Debug.Log("Onchanged value가 실행됨, IsGrabbed 값 : " + valve.IsGrabbed);
        if (photonView.IsMine)
        {
            Debug.Log("IsMine과 IsGrabbed가 통과됨");
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
