using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Domwaiter : MonoBehaviourPunCallbacks
{
    // �ڵ�, ���������� ������Ʈ
    public Transform handle;
    public Transform DomwaiterBottom;

    // ���� �ø��� ����
    private Vector3 openrotation = new Vector3(0, 0, -90); // ���� �� ȸ����
    private Vector3 closerotation = new Vector3(0, 0, 0);   // ���� �� ȸ����
    private float rotationDuration = 1.0f; // ���� ���ư��� �ð�

    // ������ �ö󰬴��� ���� Ȯ��
    private bool isOpened = false;

    private Quaternion localRotation;   // ���� ȸ��

    public void Activatehandle()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor2_SFX");

        if (isOpened)
        {
            isOpened = false;
            // ���ÿ��� ���� �ݰ�, ��Ʈ��ũ������ ����ȭ
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
        else
        {
            isOpened = true;
            // ���ÿ��� ���� �ݰ�, ��Ʈ��ũ������ ����ȭ
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
    }

    [PunRPC]
    public void RotateDoorRPC(Vector3 targetRotation)
    {
        StartCoroutine(RotateDoor(targetRotation));
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            localRotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(targetRotation), t);
            // ȸ������ ����
            transform.rotation = localRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȸ���� ���� �� ���� ȸ������ ��Ȯ�� ����
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}