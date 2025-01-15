using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class f1Tel : MonoBehaviourPunCallbacks
{
    private float endTime = 1f;

    public void EndingStart()
    {
        StartCoroutine(Ending());
    }
    
    private IEnumerator Ending()
    {
        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < endTime)
        {
            elapsedTime += Time.deltaTime;
            SceneManager.LoadScene("MainScenes");
            yield return null;
        }
    }
}
