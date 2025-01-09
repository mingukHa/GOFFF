using Photon.Pun;
using UnityEngine;

public class PlayerDead : MonoBehaviourPun
{
    private bool isDead = false;
    public delegate void CollisionEvent(bool isDead);
    public static event CollisionEvent PlayerDie;
    //string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
            isDead = true;
        Invoke("ReStart", 5f);
    }
    private void FixedUpdate()
    {
        hight();
    }
    private void hight()
    {
        if (transform.position.y >= -10f)
        {
            isDead = true;
            Invoke("ReStart", 1f);
        }
    }

    [PunRPC]
    private void ReStart()
    {
        string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel(SceneName);
    }
}
