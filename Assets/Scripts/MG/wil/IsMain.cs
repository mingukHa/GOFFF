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
                    Debug.LogError("CameraOffset�� ã�� �� �����ϴ�!");
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
                    Debug.LogError("HandOffset�� ã�� �� �����ϴ�!");
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
            Debug.Log("���� ī�޶� ��Ȱ��ȭ �Ϸ�");
        }
    }

    private void EnableControllers()
    {
        if (CameraOffset != null)
        {
            CameraOffset.SetActive(true);
            Debug.Log("�� ī�޶� Ȱ��ȭ �Ϸ�");
        }
    }

    private void DisableHand()
    {
        if (HandOffset != null)
        {
            HandOffset.SetActive(false);
            Debug.Log("���� �� ��Ȱ��ȭ �Ϸ�");
        }
    }

    private void EnableHand()
    {
        if (HandOffset != null)
        {
            HandOffset.SetActive(true);
            Debug.Log("�� �� Ȱ��ȭ �Ϸ�");
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
                Debug.Log($"{targetName} ��Ȱ��ȭ �Ϸ�");
            }
            yield return new WaitForSeconds(0.5f); // 0.5�� �������� ��õ�
        }
    }
}
