using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class Ending : MonoBehaviourPun //엔딩 스크립트
{
    private float endTime = 2f;
    public void OnButtonPress()
    {
        StartCoroutine(EndingCoroutine());       
    }
    private IEnumerator EndingCoroutine() //엔딩 코루틴
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

