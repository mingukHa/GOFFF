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
        // ���� ���ο� ���� ī�޶�� ��Ʈ�ѷ� �ʱ�ȭ
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
        // ������ ī�޶� �� ������/�ݶ��̴� ��Ȱ��ȭ
        if (!photonView.IsMine)
        {
            DisableRenderersAndColliders();
        }
    }

    private void DisableRenderersAndColliders()
    {
        // ������ ��Ȱ��ȭ
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        // �ݶ��̴� ��Ȱ��ȭ
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
}
