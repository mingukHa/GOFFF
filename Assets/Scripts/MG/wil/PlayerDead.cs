using Photon.Pun;
using UnityEngine;

public class PlayerDead : MonoBehaviourPun
{
    private bool isDead = false;
    //string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
            isDead = true;
        Invoke("ReStart", 5f);
    }

    [PunRPC]
    private void ReStart()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(SceneName);
    }
}
