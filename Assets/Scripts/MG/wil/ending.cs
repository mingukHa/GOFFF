using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ending : MonoBehaviour
{

    public void OnButtonPress()
    {
        SceneManager.LoadScene("MainScenes");
    }

    //private IEnumerator TransitionToScene()
    //{
    //    SceneManager.LoadScene("MainScenes");  // ������ ������ ��ȯ
    //    yield break;
    //}
}

