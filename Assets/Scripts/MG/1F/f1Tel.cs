using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class f1Tel : MonoBehaviourPunCallbacks
{
    private float endTime = 2f;

    public void EndingStart()
    {
        StartCoroutine(Ending());
    }
    
    private IEnumerator Ending() //���� �ڷ�ƾ
    {
        float elapsedTime = 0;        
        elapsedTime += Time.deltaTime;

        while (elapsedTime < endTime) //�ð��� endtime���� 
        {
            SceneManager.LoadScene("MainScenes"); //���ξ����� �̵�
            yield return null;
        }
    }
}
