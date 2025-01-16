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
            Debug.Log("���� �ڷ�ƾ �ѹ���");
            if (GOM == null)
            {
                GOM = FindFirstObjectByType<GameOverManagers>();
                Debug.Log($"{GOM}ã��");
            }
            yield return new WaitForSeconds(2f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ʈ���� ����");
        if (GOM != null)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("���Ϳ��� ����");
                SoundManager.instance.SFXPlay("GameOver_SFX", this.gameObject);
                GOM.ReStart();
                photonView.RPC("ReStart", RpcTarget.All);
            }
        }
    }
}
