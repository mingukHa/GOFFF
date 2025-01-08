using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LeverController : MonoBehaviourPun
{
    // �ڵ�, ���������� ������Ʈ
    public Transform handle;
    public GameObject elevator;

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
            StartCoroutine(RotateHandle());

        // ���������� Ȱ��ȭ
        if (elevator != null)
            elevator.SetActive(true);
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
}
