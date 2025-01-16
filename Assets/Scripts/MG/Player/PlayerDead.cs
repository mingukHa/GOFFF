using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Start()
    {
        Debug.Log("게임 오버 매니저 활성화");
        GOM = GetComponent<GameOverManagers>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (GOM != null)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("몬스터에게 닿음");
                GOM.ReStart();
                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
