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
            // �ٸ� �÷��̾��� ��Ʈ�ѷ��� ������ �� ��ȣ�ۿ� ��Ȱ��ȭ
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

        Debug.Log("��Ȱ��ȭ �߽�");
    }

    private void EnableControllers()
    {
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("Ȱ��ȭ �߽�");
    }
    private void Disablehand()
    {
        if (CameraOffset != null) CameraOffset.SetActive(false);

        Debug.Log("��Ȱ��ȭ �߽�");
    }

    private void Enablehand()
    {
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("Ȱ��ȭ �߽�");
    }
}