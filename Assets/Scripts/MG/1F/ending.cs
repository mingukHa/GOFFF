using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class Ending : MonoBehaviourPun //���� ��ũ��Ʈ
{
    private float endTime = 2f;
    public void OnButtonPress()
    {
        StartCoroutine(EndingCoroutine());       
    }
    private IEnumerator EndingCoroutine() //���� �ڷ�ƾ
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

