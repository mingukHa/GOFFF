using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject CameraOffset;
    

    private void Start()
    {
        // �������� ��Ʈ�ѷ� ã��
        CameraOffset = transform.Find("Cameras")?.gameObject;
        

        // PhotonView�� Ȯ���Ͽ� ���� �÷��̾ ��Ʈ�ѷ��� Ȱ��ȭ
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
        // ���� �÷��̾ �ƴ� ��� ��Ʈ�ѷ� ��Ȱ��ȭ
        if (CameraOffset != null) CameraOffset.SetActive(false);
        
        Debug.Log("��Ȱ��ȭ �߽�");
    }

    private void EnableControllers()
    {
        // ���� �÷��̾��� ��Ʈ�ѷ� Ȱ��ȭ
        if (CameraOffset != null) CameraOffset.SetActive(true);
        Debug.Log("Ȱ��ȭ �߽�");
    }
}
