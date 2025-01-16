using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LeverController : MonoBehaviourPun
{
    public Transform lever; // ����
    public BoxCollider eVButton; // ���������� ��� ��ư �ݶ��̴�

    // ���� �ø��� ����
    private Vector3 targetRotation = new Vector3(-45, 0, 0); // �ö� ������ ȸ����
    private float rotationDuration = 1.0f; // ���� ��� �ҿ� �ð�

    // ������ �ö󰬴��� ���� Ȯ��
    private bool isActivated = false;

    // ������ Simple Interactor�� �۵��ϸ� ����
    public void ActivateLever()
    {
        // ������ �̹� �۵��ߴٸ� return
        if (isActivated) return;

        // ������ 1ȸ�� �۵��ϵ��� ���¸� true�� ����
        isActivated = true;

        // RPC�� RPC_RotateHandle �޼ҵ� ȣ��
        if (lever != null)
        {
            photonView.RPC("RPC_RotateHandle", RpcTarget.All);
        }

        // RPC�� RPC_ActivateEVButton �޼ҵ� ȣ��
        if (eVButton != null)
        {
            photonView.RPC("RPC_ActivateEVButton", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_RotateLever()
    {
        StartCoroutine(RotateLever());
    }

    private IEnumerator RotateLever()
    {
        Quaternion initialRotation = lever.localRotation; // ���� �ʱ� ȸ����
        Quaternion finalRotation = Quaternion.Euler(targetRotation); // �ö� ���� ȸ����

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            // ���� ��� �ҿ� �ð���ŭ Slerp�� �����Ͽ� �ε巯�� ������ ����
            lever.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lever.localRotation = finalRotation; // ������ ȸ������ ���� ȸ������ ����ȭ
        SoundManager.instance.SFXPlay("Spark1_SFX", this.gameObject);
    }

    [PunRPC]
    private void RPC_ActivateEVButton()
    {
        // ���������� ��� ��ư �ݶ��̴� Ȱ��ȭ
        eVButton.enabled = true;
    }
}
