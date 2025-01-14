using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject _cameraOffset;
    private GameObject _handOffset;

    // Lazy Initialization
    private GameObject CameraOffset
    {
        get
        {
            if (_cameraOffset == null)
            {
                _cameraOffset = transform.Find("Cameras")?.gameObject;
                if (_cameraOffset == null)
                {
                    Debug.LogError("CameraOffset을 찾을 수 없습니다!");
                }
            }
            return _cameraOffset;
        }
    }

    private GameObject HandOffset
    {
        get
        {
            if (_handOffset == null)
            {
                _handOffset = transform.Find("handOffset")?.gameObject;
                if (_handOffset == null)
                {
                    Debug.LogError("HandOffset을 찾을 수 없습니다!");
                }
            }
            return _handOffset;
        }
    }

    private void Awake()
    {
        // 소유 여부에 따라 카메라와 컨트롤러 초기화
        if (!photonView.IsMine)
        {
            DisableControllers();
            DisableHand();
        }
        else
        {
            EnableControllers();
            EnableHand();
        }
    }

    private void Start()
    {
        // 상대방의 카메라 및 렌더러/콜라이더 비활성화
        if (!photonView.IsMine)
        {
            DisableRenderersAndColliders();
        }
    }

    private void DisableRenderersAndColliders()
    {
        // 렌더러 비활성화
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        // 콜라이더 비활성화
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
    }

    private void DisableControllers()
    {
        if (CameraOffset != null)
        {
            CameraOffset.SetActive(false);
            Debug.Log("상대방 카메라 비활성화 완료");
        }
    }

    private void EnableControllers()
    {
        if (CameraOffset != null)
        {
            CameraOffset.SetActive(true);
            Debug.Log("내 카메라 활성화 완료");
        }
    }

    private void DisableHand()
    {
        if (HandOffset != null)
        {
            HandOffset.SetActive(false);
            Debug.Log("상대방 손 비활성화 완료");
        }
    }

    private void EnableHand()
    {
        if (HandOffset != null)
        {
            HandOffset.SetActive(true);
            Debug.Log("내 손 활성화 완료");
        }
    }
}
