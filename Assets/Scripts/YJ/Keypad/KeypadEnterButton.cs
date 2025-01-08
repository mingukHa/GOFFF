using Photon.Pun;
using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager;

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
    }

    public void OnPressed()
    {
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("OnEnterButtonPressed", RpcTarget.All);
    }
}
