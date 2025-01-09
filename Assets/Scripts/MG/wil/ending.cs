using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Pun;

public class ending : MonoBehaviourPun
{

    public void OnButtonPress()
    {       
        SceneManager.LoadScene("JHScenes2");
    }

    
}

