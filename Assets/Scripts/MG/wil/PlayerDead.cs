using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Awake()
    {
        GOM = GetComponent<GameOverManagers>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Invoke("ReStart", 2f); // 2초 뒤 RPC 호출
        }
    }  
}
