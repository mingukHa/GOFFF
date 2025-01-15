using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    [SerializeField] private GameOverManagers GOM;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Monster"))
        {
            Debug.Log("몬스터에게 닿음");
            if (GOM != null)
            {
                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
