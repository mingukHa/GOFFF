using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Awake()
    {
        GOM = GetComponent<GameOverManagers>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Monster"))
        {
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }  
}
