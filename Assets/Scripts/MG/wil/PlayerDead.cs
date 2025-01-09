using Photon.Pun;
using UnityEngine;

public class PlayerDead : MonoBehaviourPun
{
      
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))           
        Invoke("ReStart", 5f);
    }
    [PunRPC]
    private void ReStart()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(SceneName);        
    }
}
