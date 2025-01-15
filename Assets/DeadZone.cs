using Photon.Pun;
using UnityEngine;

public class DeadZone : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Start()
    {
        GOM = GetComponent<GameOverManagers>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        

        if (collider.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.SFXPlay("Scream_SFX", this.gameObject);
            GOM.ReStart();
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }
}
