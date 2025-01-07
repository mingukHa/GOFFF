using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject cameraOffset;
    private GameObject handOffset;

    private void Awake()
    {
        // Find CameraOffset and HandOffset objects
        cameraOffset = transform.Find("Cameras")?.gameObject;
        handOffset = transform.Find("handOffset")?.gameObject;

        // 로컬 플레이어와 상대방 처리
        if (!photonView.IsMine)
        {
            DisableControllers(); // 카메라 및 컨트롤러 비활성화
            DisableRenderers(); // 렌더링 비활성화
            DisableColliders(); // 충돌 비활성화
            DisableInteractors(); // XR Interactor 비활성화
        }
        else
        {
            EnableControllers(); // 카메라 및 컨트롤러 활성화
        }
    }

    private void Start()
    {
        // 이미 Awake에서 처리했지만, 추가 확인 로직
        if (!photonView.IsMine)
        {
            DisableRenderers();
            DisableColliders();
        }
    }

    private void DisableControllers()
    {
        if (cameraOffset != null) cameraOffset.SetActive(false);
        if (handOffset != null) handOffset.SetActive(false);

        Debug.Log("비활성화: 컨트롤러와 카메라 오프셋");
    }

    private void EnableControllers()
    {
        if (cameraOffset != null) cameraOffset.SetActive(true);
        if (handOffset != null) handOffset.SetActive(true);

        Debug.Log("활성화: 컨트롤러와 카메라 오프셋");
    }

    private void DisableRenderers()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        Debug.Log("비활성화: 렌더링");
    }

    private void DisableColliders()
    {
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        Debug.Log("비활성화: 충돌");
    }

    private void DisableInteractors()
    {
        foreach (var interactor in GetComponentsInChildren<XRBaseInteractor>())
        {
            interactor.enabled = false;
        }

        Debug.Log("비활성화: XR Interactor");
    }
}
