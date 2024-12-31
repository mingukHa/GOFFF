using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject leftController;
    private GameObject rightController;

    private void Start()
    {
        // 동적으로 컨트롤러 찾기
        leftController = transform.Find("LeftController")?.gameObject;
        rightController = transform.Find("RightController")?.gameObject;

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
        if (leftController != null) leftController.SetActive(false);
        if (rightController != null) rightController.SetActive(false);
        Debug.Log("비활성화 했슈");
    }

    private void EnableControllers()
    {
        // 로컬 플레이어의 컨트롤러 활성화
        if (leftController != null) leftController.SetActive(true);
        if (rightController != null) rightController.SetActive(true);
        Debug.Log("활성화 했슈");
    }
}
