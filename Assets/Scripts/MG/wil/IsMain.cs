using Photon.Pun;
using UnityEngine;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject CameraOffset;
    private GameObject handOffset;

    private void Start()
    {
        CameraOffset = transform.Find("Cameras")?.gameObject;
        handOffset = transform.Find("handOffset")?.gameObject;
    }

    private void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (photonView.IsMine)
        {
            EnableControllers();
            Enablehand();
        }
        else
        {
            DisableControllers();
            Disablehand();
        }
    }

    private void DisableControllers()
    {
        if (CameraOffset != null) CameraOffset.SetActive(false);
        Debug.Log("비활성화 했슈");
    }

    private void EnableControllers()
    {
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("활성화 했슈");
    }

    private void Disablehand()
    {
        if (handOffset != null) handOffset.SetActive(false);
        Debug.Log("비활성화 했슈");
    }

    private void Enablehand()
    {
        if (handOffset != null) handOffset.SetActive(true);
        Debug.Log("활성화 했슈");
    }
}
