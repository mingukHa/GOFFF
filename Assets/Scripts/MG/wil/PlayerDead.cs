using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    [SerializeField] private GameOverManagers GOM;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Monster"))
        {
            Debug.Log("���Ϳ��� ����");
            if (GOM != null)
            {
                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
