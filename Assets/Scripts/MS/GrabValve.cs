using Photon.Pun;
using UnityEngine;

public class GrabValve : MonoBehaviourPun
{
    // 파이프에 따라 이벤트를 따로 등록
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;
    public delegate void grabValve2Delegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValve2Trigger;

    private bool isGrabbed = false;

    private Vector3 currentTR = Vector3.zero;

    private void Start()
    {
        currentTR = new Vector3(-30f, 0, -27f);
    }

    private void Update()
    {
        // 밸브가 추락할 경우 currentTr 장소에 스폰
        if(-15f>transform.position.y)
        {
            Rigidbody ValveRD=transform.GetComponent<Rigidbody>();
            ValveRD.angularVelocity = Vector3.zero;
            ValveRD.linearVelocity = Vector3.zero;
            transform.position = currentTR;
            transform.rotation = Quaternion.identity;
        }
    }

    // 다른 Collider가 이 밸브와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (isGrabbed)
        {
            if (other.name == "CylinderA")
            {
                grabValveTrigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger", RpcTarget.Others ,other);
            }
            else if (other.name == "CylinderB")
            {
                grabValve2Trigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger2", RpcTarget.Others, other);
            }
        }
    }

    [PunRPC]
    private void RPCTrigger(Collider other)
    {
        grabValveTrigger?.Invoke(gameObject, other);
    }

    [PunRPC]
    private void RPCTrigger2(Collider other)
    {
        grabValve2Trigger?.Invoke(gameObject, other);
    }

    public void SelectOn()
    {
        Debug.Log("밸브를 잡았습니다.");
        isGrabbed = true;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, true);

    }

    public void SelectOff()
    {
        Debug.Log("밸브를 놓았습니다.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    // grab이 됐을경우에 isKinematic이 다른 클라이언트에서는 켜져있지 않아
    // 중력의 영향을 받기 때문에 잡았을 경우 다른 클라이언트에서 isKinematic이 켜지도록 함
    [PunRPC]
    private void RPCGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
        Rigidbody grabValveRD = transform.GetComponent<Rigidbody>();
        if (grabValveRD != null)
        {
            grabValveRD.isKinematic = grabbed;
        }
    }

}
