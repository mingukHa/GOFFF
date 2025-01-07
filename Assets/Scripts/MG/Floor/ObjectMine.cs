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
        Debug.Log("오브젝트를 집었음");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
        Debug.Log("오브젝트가 닿았음");
    }

}
