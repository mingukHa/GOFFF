using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabValve : MonoBehaviourPun
{
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;

    private bool isGrabbed = false;


    // �ٸ� Collider�� �� ���� �浹���� �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if(!isGrabbed)
            grabValveTrigger?.Invoke(gameObject,other);
    }

    public void SelectOn()
    {
        Debug.Log("��긦 ��ҽ��ϴ�.");
        isGrabbed = true;
        photonView.RPC("RPCGrabbed", RpcTarget.OthersBuffered, true);

    }

    public void SelectOff()
    {
        Debug.Log("��긦 ���ҽ��ϴ�.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.OthersBuffered, false);
    }

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    [PunRPC]
    private void RPCGrabbed(bool Grabbed)
    {
        isGrabbed = Grabbed;
    }

}
