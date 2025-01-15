using Photon.Pun;
using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager; // Ű�е� �Ŵ���

    private void Start()
    {
        keypadManager = FindFirstObjectByType<KeypadManager>();
    }

    public void OnPressed()
    {
        // Ű�е� ������ SFX �߰�

        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("OnEnterButtonPressed", RpcTarget.Others);
    }
}
