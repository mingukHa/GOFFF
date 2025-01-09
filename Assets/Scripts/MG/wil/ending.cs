using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ending : MonoBehaviour
{

    public void OnButtonPress()
    {
        SceneManager.LoadScene("JHScenes2");
    }

    //private IEnumerator TransitionToScene()
    //{
    //    SceneManager.LoadScene("MainScenes");  // 지정된 씬으로 전환
    //    yield break;
    //}
}

