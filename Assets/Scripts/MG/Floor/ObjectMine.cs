using Photon.Pun;
using UnityEngine;

public class ObjectMain : MonoBehaviourPun
{
    private bool isGrabbed = false;
    public void OnSelectEnter()
    {
        if(!photonView.IsMine)
        {
            photonView.RequestOwnership();
            isGrabbed = true;
            photonView.RPC("RPCGrabbed", RpcTarget.Others, true);
        }
        Debug.Log("오브젝트를 집었음");
    }
    public void OnSelectFalse()
    {
        Debug.Log("물체를 놓았습니다.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("오브젝트가 닿았음");
    }
    [PunRPC]
    private void RPCGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
        Rigidbody grabValveRD = transform.GetComponent<Rigidbody>();
        if (grabValveRD != null)
        {
            grabValveRD.useGravity = !grabbed;
        }
    }

}
