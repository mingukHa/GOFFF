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
        Debug.Log("트리거 활성화");
        PhotonNetwork.Destroy(ironcage);

        PhotonNetwork.Destroy(ironcage2);
    }
    private void Ironcage()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
