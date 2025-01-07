using Photon.Pun;
using UnityEngine;

public class controllerOff : MonoBehaviourPun
{
    public GameObject handOffset;

    private void Awake()
    {

        if (!photonView.IsMine)
        {
            handOffset.SetActive(false);
        }

    }
}
