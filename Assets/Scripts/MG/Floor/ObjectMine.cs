using Photon.Pun;
using UnityEngine;

public class ObjectMain : MonoBehaviourPun
{
    public void OnSelectEnter()
    {
        if(!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("������Ʈ�� ������");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("������Ʈ�� �����");
    }

}
