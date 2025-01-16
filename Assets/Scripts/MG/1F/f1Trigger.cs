using Photon.Pun;
using UnityEngine;

public class f1Trigger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject ironcage;
    [SerializeField]
    private GameObject ironcage2;
    [SerializeField]
    private GameObject ironcage3;
    [SerializeField]
    private GameObject ironcage4;

    private void OnTriggerEnter(Collider other)
    {
        Ironcage();
        photonView.RPC("Ironcage", RpcTarget.Others);

    }
    [PunRPC]
    private void Ironcage()
    {
        PhotonNetwork.Destroy(ironcage);

        PhotonNetwork.Destroy(ironcage2);
        PhotonNetwork.Destroy(ironcage3);

        PhotonNetwork.Destroy(ironcage4);
    }
}
