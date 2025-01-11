using Photon.Pun;
using UnityEngine;

public class f1Trigger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject ironcage;
    [SerializeField]
    private GameObject ironcage2;

    private void OnTriggerEnter(Collider other)
    {
        Ironcage();
        photonView.RPC("Ironcage", RpcTarget.Others);
    }
    private void Ironcage()
    {
        PhotonNetwork.Destroy(ironcage);

        PhotonNetwork.Destroy(ironcage2);
    }
}
