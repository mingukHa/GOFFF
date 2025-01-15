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
    private IEnumerator Ending() //엔딩 코루틴
    {
        float elapsedTime = 0;
        elapsedTime += Time.deltaTime;

        while (elapsedTime < endTime) //시간이 endtime까지 
        {
            SceneManager.LoadScene("MainScenes"); //메인씬으로 이동
            yield return null;
        }
    }

}

