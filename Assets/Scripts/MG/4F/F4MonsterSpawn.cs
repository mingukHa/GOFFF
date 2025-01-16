using Photon.Pun;
using UnityEngine;

public class F4MonsterSpawn : MonoBehaviourPun
{
    [SerializeField] private GameObject zombi;

    public void Zombi()
    {
        Zombi();
        photonView.RPC("Zombi", RpcTarget.All);
    }

    [PunRPC]
    private void ZombiOn()
    {
        zombi.SetActive(true);
    }
}
