using Photon.Pun;
using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager; // Ű�е� �Ŵ���
    private SoundManager soundManager; // ���� �Ŵ���

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
