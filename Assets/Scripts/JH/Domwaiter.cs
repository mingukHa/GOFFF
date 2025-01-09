using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Domwaiter : MonoBehaviourPunCallbacks
{
    // �ڵ�, ���������� ������Ʈ
    public Transform handle;
    public Transform DomwaiterBottom;

    // �� ����/���� ����
    public float openAngle = -90f; // ���� �� ȸ����

    // ���� �ø��� ����
    private Vector3 openrotation;
    private Vector3 closerotation = Vector3.zero;   // ���� �� ȸ����

    private float rotationDuration = 1.0f; // ���� ���ư��� �ð�

    // ������ �ö󰬴��� ���� Ȯ��
    private bool isOpened = false;

    private Quaternion localRotation;   // ���� ȸ��

    private void Start()
    {
        // ���� �� ���� �ʱ�ȭ
        openrotation = new Vector3(0, 0, openAngle);
    }

    public void Activatehandle()
    {
        if (isOpened)
        {
            isOpened = false;
            // ���ÿ��� ���� ���������� �ݰ�, ��Ʈ��ũ������ ����ȭ
            RotateDoorLocally(closerotation); // ���ÿ��� �� �ݱ�
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
        else
        {
            isOpened = true;
            // ���ÿ��� ���� ���������� ����, ��Ʈ��ũ������ ����ȭ
            RotateDoorLocally(openrotation); // ���ÿ��� �� �ݱ�
            photonView.RPC("RotateDoorRPC", RpcTarget.All, openrotation);
        }
    }

    private void RotateDoorLocally(Vector3 targetRotation)
    {
        StartCoroutine(RotateDoor(targetRotation));
    }

    [PunRPC]
    public void RotateDoorRPC(Vector3 targetRotation)
    {
        RotateDoorLocally(targetRotation);
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

        while (elapsedTime < rotationDuration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsedTime / rotationDuration);
            transform.rotation = Quaternion.Lerp(initialRotation, targetQuaternion, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ڷ�ƾ�� ���� �� ���� ȸ�� �� ����
        transform.rotation = targetQuaternion;
    }
}