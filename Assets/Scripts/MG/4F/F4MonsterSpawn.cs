using Photon.Pun;
using UnityEngine;

public class F4MonsterSpawn : MonoBehaviourPun //4층 몬스터 스폰
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
