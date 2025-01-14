using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject _cameraOffset;
    private GameObject _handOffset;

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
        if (!photonView.IsMine)
        {
            StartCoroutine(EnsureDisabled(CameraOffset, "CameraOffset"));
            StartCoroutine(EnsureDisabled(HandOffset, "HandOffset"));
        }
        else
        {
            EnableControllers();
            EnableHand();
        }
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            DisableRenderersAndColliders();
        }
    }

    private void DisableRenderersAndColliders()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

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

    private IEnumerator EnsureDisabled(GameObject target, string targetName)
    {
        while (target == null || target.activeSelf)
        {
            target = transform.Find(targetName)?.gameObject;
            if (target != null && target.activeSelf)
            {
                target.SetActive(false);
                Debug.Log($"{targetName} 비활성화 완료");
            }
            yield return new WaitForSeconds(0.5f); // 0.5초 간격으로 재시도
        }
    }
}
