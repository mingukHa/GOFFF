using Photon.Pun;
using UnityEngine;

public class PlayerCameraSetup : MonoBehaviourPunCallbacks
{
    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();

        if (photonView.IsMine)
        {
            // �� �÷��̾��� ī�޶� Ȱ��ȭ
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            // �ٸ� �÷��̾��� ī�޶�� ��Ȱ��ȭ
            playerCamera.gameObject.SetActive(false);
        }
    }
}