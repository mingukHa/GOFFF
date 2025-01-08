using Photon.Pun;
using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager; // 키패드 매니저
    private SoundManager soundManager; // 사운드 매니저

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void OnPressed()
    {
        SoundManager.instance.SFXPlay("Button2_SFX");


        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("OnEnterButtonPressed", RpcTarget.Others);
    }
}
