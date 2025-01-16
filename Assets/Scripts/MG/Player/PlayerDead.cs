using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerDead : MonoBehaviourPun
{
    private GameOverManagers GOM;
    private void Start()
    {
        StartCoroutine(FindGOM());
    }

    private IEnumerator FindGOM()
    {
        while (true)
        {
            if (GOM == null)
                GOM = FindFirstObjectByType<GameOverManagers>();
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnCollisonEnter(Collider other)
    {
        if (GOM != null)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("몬스터에게 닿음");
                SoundManager.instance.SFXPlay("GameOver_SFX", this.gameObject);
                GOM.ReStart();
                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
