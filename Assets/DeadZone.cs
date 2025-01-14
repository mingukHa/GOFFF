using Photon.Pun;
using UnityEngine;

public class DeadZone : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Awake()
    {
        GOM = GetComponent<GameOverManagers>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GOM.ReStart();
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }
}
