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
        SoundManager.instance.SFXPlay("Scream_SFX", this.gameObject);

        if (collider.gameObject.CompareTag("Player"))
        {
            GOM.ReStart();
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }
}
