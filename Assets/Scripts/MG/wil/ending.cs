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
    //    SceneManager.LoadScene("MainScenes");  // ������ ������ ��ȯ
    //    yield break;
    //}
}

