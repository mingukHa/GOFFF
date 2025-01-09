using Photon.Pun;
using UnityEngine;

public class PlayerDead : MonoBehaviourPun
{
    private bool isDead = false;
      
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
            isDead = true;
        Invoke("ReStart", 5f);
    }
    public void Deadzone()
    {
        isDead = true;
        ReStart();
    }
    

    [PunRPC]
    private void ReStart()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(SceneName);
        
    }
}
