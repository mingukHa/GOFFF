using Photon.Pun;
using UnityEngine;

public class DeadZone : MonoBehaviourPun
{   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            photonView.RPC("RestartLevel", RpcTarget.All);
        }
    }

    private void RestartLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel(sceneName);
        }
    }
}
