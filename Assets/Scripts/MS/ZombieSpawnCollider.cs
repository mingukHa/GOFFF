using Photon.Pun;
using UnityEngine;

public class ZombieSpawnCollider : MonoBehaviourPun
{
    [SerializeField]
    private GameObject zombie;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !zombie.activeSelf)
        {
            photonView.RPC("SpawnZombieTrigger", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SpawnZombieTrigger()
    {
        zombie.SetActive(true);
    }
}
