using Photon.Pun;
using UnityEngine;

public class ValveTrigger : MonoBehaviourPun
{
    public GameObject cage; //케이지 터질 것
    
    private void OnCollisionEnter(Collision collision) //콜리전 엔터가 되면
    {
        if (collision.gameObject.CompareTag("Player")) //플레이어 태그 받으면
        {
            CageDestroy(); //케이지 터트리고
            photonView.RPC("CageDestroy", RpcTarget.Others); //다른 사람에게 알림
        }
    }
    [PunRPC]
    private void CageDestroy()
    {
        PhotonNetwork.Destroy(cage);
        SoundManager.instance.SFXPlay("JailCrack_SFX", this.gameObject);
    }

    
}
