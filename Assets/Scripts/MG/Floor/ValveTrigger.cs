using Photon.Pun;
using UnityEngine;

public class ValveTrigger : MonoBehaviourPun
{
    public GameObject cage;
    private void OnTriggerEnter(Collider other)
    {
        PhotonNetwork.Destroy(cage);
    }
}
