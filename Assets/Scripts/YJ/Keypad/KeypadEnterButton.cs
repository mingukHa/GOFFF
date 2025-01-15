using Photon.Pun;
using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager; // 키패드 매니저

    private void Start()
    {
        keypadManager = FindFirstObjectByType<KeypadManager>();
    }

    public void OnPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);

        // RPC를 통해 엔터 버튼 누르는 동작을 동기화
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("OnEnterButtonPressed", RpcTarget.Others);
    }
}
