using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPun
{
    private GameObject leftController;
    private GameObject rightController;

    private void Start()
    {
        // �������� ��Ʈ�ѷ� ã��
        leftController = transform.Find("LeftController")?.gameObject;
        rightController = transform.Find("RightController")?.gameObject;

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
        if (leftController != null) leftController.SetActive(false);
        if (rightController != null) rightController.SetActive(false);
        Debug.Log("��Ȱ��ȭ �߽�");
    }

    private void EnableControllers()
    {
        // ���� �÷��̾��� ��Ʈ�ѷ� Ȱ��ȭ
        if (leftController != null) leftController.SetActive(true);
        if (rightController != null) rightController.SetActive(true);
        Debug.Log("Ȱ��ȭ �߽�");
    }
}
