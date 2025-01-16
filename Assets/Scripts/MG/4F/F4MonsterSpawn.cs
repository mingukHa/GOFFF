using Photon.Pun;
using UnityEngine;

public class F4MonsterSpawn : MonoBehaviourPun
{
    [SerializeField] private GameObject zombi;

    public void Zombi()
    {
        ZombieOn();
        photonView.RPC("ZombieOn", RpcTarget.All);
    }

    [PunRPC]
    private void ZombieOn()
    {
        zombi.SetActive(true);
        SoundManager.instance.SFXPlay("ZomShout_SFX", zombi.gameObject);
    }
}
