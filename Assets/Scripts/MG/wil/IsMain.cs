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

        // ���� �÷��̾�� ���� ó��
        if (!photonView.IsMine)
        {
            DisableControllers(); // ī�޶� �� ��Ʈ�ѷ� ��Ȱ��ȭ
            DisableRenderers(); // ������ ��Ȱ��ȭ
            DisableColliders(); // �浹 ��Ȱ��ȭ
            DisableInteractors(); // XR Interactor ��Ȱ��ȭ
        }
        else
        {
            EnableControllers(); // ī�޶� �� ��Ʈ�ѷ� Ȱ��ȭ
        }
    }

    private void Start()
    {
        // �̹� Awake���� ó��������, �߰� Ȯ�� ����
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

        Debug.Log("��Ȱ��ȭ: ��Ʈ�ѷ��� ī�޶� ������");
    }

    private void EnableControllers()
    {
        if (cameraOffset != null) cameraOffset.SetActive(true);
        if (handOffset != null) handOffset.SetActive(true);

        Debug.Log("Ȱ��ȭ: ��Ʈ�ѷ��� ī�޶� ������");
    }

    private void DisableRenderers()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        Debug.Log("��Ȱ��ȭ: ������");
    }

    private void DisableColliders()
    {
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        Debug.Log("��Ȱ��ȭ: �浹");
    }

    private void DisableInteractors()
    {
        foreach (var interactor in GetComponentsInChildren<XRBaseInteractor>())
        {
            interactor.enabled = false;
        }

        Debug.Log("��Ȱ��ȭ: XR Interactor");
    }
}
