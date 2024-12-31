using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private void Start()
    {

        JoinEchoChannel.Instance.OnLoginEndEvent += HandleLoginEnd;

    }

    //초기화가 끝난 후 씬 전환
    private void HandleLoginEnd()
    {

        SceneManager.LoadScene("Game");

    }
}