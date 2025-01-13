using Photon.Pun;
using UnityEngine;

public class ValveTrigger : MonoBehaviourPun
{
    public GameObject cage;
   
    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("DestroyCage", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyCage()
    {
        PhotonNetwork.Destroy(cage);
    }
}
