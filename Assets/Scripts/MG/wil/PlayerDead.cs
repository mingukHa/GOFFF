using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            photonView.RPC(nameof(NotifyDeath), RpcTarget.All); 
        }
    }

    [PunRPC]
    public void NotifyDeath()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            Invoke(nameof(RestartLevel), 2f); 
        }
    }

    private void RestartLevel()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(sceneName); 
    }
}
