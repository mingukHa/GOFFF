using Photon.Pun;
using UnityEngine;

public class ValveTrigger : MonoBehaviourPun
{
    public GameObject cage;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 활성화");
        PhotonNetwork.Destroy(cage);
    }
}
