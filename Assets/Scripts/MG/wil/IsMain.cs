using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject CameraOffset;
    

    private void Start()
    {
        CameraOffset = transform.Find("Cameras")?.gameObject;

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
        if (CameraOffset != null) CameraOffset.SetActive(false);
        
        Debug.Log("��Ȱ��ȭ �߽�");
    }

    private void EnableControllers()
    {
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("Ȱ��ȭ �߽�");
    }
}
