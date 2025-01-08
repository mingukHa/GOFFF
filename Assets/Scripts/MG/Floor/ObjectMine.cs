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
        Debug.Log("������Ʈ�� ������");
    }
    public void OnSelectFalse()
    {
        Debug.Log("��ü�� ���ҽ��ϴ�.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("������Ʈ�� �����");
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
