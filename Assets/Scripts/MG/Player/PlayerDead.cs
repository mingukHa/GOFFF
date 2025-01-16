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
            Debug.Log("데드 코루틴 한바퀴");
            if (GOM == null)
            {
                GOM = FindFirstObjectByType<GameOverManagers>();
                Debug.Log($"{GOM}찾음");
            }
            yield return new WaitForSeconds(2f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 닿음");
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
