using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Awake()
    {
        GOM = GetComponent<GameOverManagers>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (GOM != null)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("���Ϳ��� ����");

                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
