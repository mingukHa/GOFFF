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
        SoundManager.instance.SFXPlay("Button2_SFX");

        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("OnEnterButtonPressed", RpcTarget.All);
    }
}
