using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class ending : MonoBehaviourPun
{
    private float endTime = 2f;
    public void OnButtonPress()
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

