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
        if (GOM != null)
        {
            if (collider.gameObject.CompareTag("Monster"))
            {
                Debug.Log("몬스터에게 닿음");

                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
