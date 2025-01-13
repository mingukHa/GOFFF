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
            // 내 플레이어의 카메라만 활성화
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            // 다른 플레이어의 카메라는 비활성화
            playerCamera.gameObject.SetActive(false);
        }
    }
}