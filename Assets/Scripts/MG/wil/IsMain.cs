using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject CameraOffset;
    private GameObject handOffset;


    private void Awake()
    {
        CameraOffset = transform.Find("Cameras")?.gameObject;
        handOffset = transform.Find("handOffset")?.gameObject;

        if (!photonView.IsMine)
        {
            DisableControllers();
            photonView.RequestOwnership();
            Disablehand();
        }
        else
        {
            EnableControllers();
            Enablehand();
        }
    }
    private void Start()
    {
        if (!photonView.IsMine)
        {
            // 다른 플레이어의 컨트롤러는 렌더링 및 상호작용 비활성화
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }

            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
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
        if (CameraOffset != null) CameraOffset.SetActive(false);

        Debug.Log("비활성화 했슈");
    }

    private void Enablehand()
    {
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("활성화 했슈");
    }
}