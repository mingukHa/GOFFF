using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject CameraOffset;
    

    private void Start()
    {
        // 동적으로 컨트롤러 찾기
        CameraOffset = transform.Find("Cameras")?.gameObject;
        

        // PhotonView를 확인하여 로컬 플레이어만 컨트롤러를 활성화
        if (!photonView.IsMine)
        {
            DisableControllers();
        }
        else
        {
            EnableControllers();
        }
    }

    private void DisableControllers()
    {
        // 로컬 플레이어가 아닌 경우 컨트롤러 비활성화
        if (CameraOffset != null) CameraOffset.SetActive(false);
        
        Debug.Log("비활성화 했슈");
    }

    private void EnableControllers()
    {
        // 로컬 플레이어의 컨트롤러 활성화
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("활성화 했슈");
    }
}
