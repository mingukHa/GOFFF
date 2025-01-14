using Photon.Pun;
using UnityEngine;

public class controllerOff : MonoBehaviourPun
{
    public GameObject handOffset;
    public GameObject Cameras;
    private void Awake()
    {

        if (!photonView.IsMine)
        {
            handOffset.SetActive(false);
            Cameras.SetActive(false);
        }

    }
}
