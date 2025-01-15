using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LeverController : MonoBehaviourPun
{
    // �ڵ�, ���������� ������Ʈ
    public Transform handle; // ���� �ڵ�
    public BoxCollider eVButton; // ���������� ��� ��ư

    // ���� �ø��� ����
    private Vector3 targetRotation = new Vector3(-45, 0, 0); // �ö� ������ ȸ����
    private float rotationDuration = 1.0f; // ������ �ö󰡴� �ӵ�

    // ������ �ö󰬴��� ���� Ȯ��
    private bool isActivated = false;

    public void ActivateLever()
    {
        if (isActivated) return; // ������ 1ȸ�� �۵��ϵ��� ��

        isActivated = true;

        // ���� ȸ��
        if (handle != null)
        {
            photonView.RPC("RPC_RotateHandle", RpcTarget.All);
        }

        // ���������� ��ư Ȱ��ȭ
        photonView.RPC("RPC_ActivateEVButton", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_RotateHandle()
    {
        StartCoroutine(RotateHandle());
    }

    private IEnumerator RotateHandle()
    {
        Quaternion initialRotation = handle.localRotation;
        Quaternion finalRotation = Quaternion.Euler(targetRotation);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            handle.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        handle.localRotation = finalRotation; // ���� ȸ���� Ȯ��
    }

    [PunRPC]
    private void RPC_ActivateEVButton()
    {
        eVButton.enabled = true;
    }
}
