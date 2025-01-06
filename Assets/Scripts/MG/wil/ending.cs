using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ending : MonoBehaviour
{

    public void OnButtonPress()
    {
        StartCoroutine(TransitionToScene());
    }

    private IEnumerator TransitionToScene()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SceneManager.LoadScene("MainScenes");  // 지정된 씬으로 전환
    }
}

