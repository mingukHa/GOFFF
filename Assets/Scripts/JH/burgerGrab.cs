using Photon.Pun;
using UnityEngine;

public class burgerGrab : MonoBehaviourPun
{
    public void OnSelectBurger()
    {
        if(!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }
}