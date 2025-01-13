using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Domwaiter : MonoBehaviourPun
{
    public Transform ovenDoor;  // ���� �� ������Ʈ
    private bool isOpen = false;    
    private bool isRotating = false; // ȸ�� ���̸� �߰� Ʈ���Ÿ� ����
    public float rotationDuration = 1f;  // ���� ȸ���ϴ� �ð� (�ν����Ϳ��� ���� ����)

    public void ToggleDoor()
    {
        if (isRotating) return;  // ���� ȸ�� ���̸� ������� ����

        photonView.RPC("RPCToggleDoor", RpcTarget.All);
    }

    // ��Ʈ��ũ�� ȣ��Ǵ� �� ���� ���� �Լ� (RPC)
    [PunRPC]
    public void RPCToggleDoor()
    {
        // ���� �� ���� üũ
        float targetRotation = isOpen ? 0f : 90f;
        if (Mathf.Approximately(ovenDoor.localRotation.eulerAngles.y, targetRotation)) return;

        StartCoroutine(RotateDoor(isOpen ? 0f : 90f));  // ���� ������ �ݱ�, �ƴϸ� ����

        SoundManager.instance.SFXPlay("OpenDoor_SFX");  // �� ���� ���� ���
    }

    // �� ȸ�� �ڷ�ƾ
    private IEnumerator RotateDoor(float targetRotation)
    {
        //ȸ���ϰڴ�!
        isRotating = true;

        //ȸ�� �ʱ�, ���, �ð� �񱳰�
        Quaternion startRotation = ovenDoor.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, targetRotation, 0f);
        float timeElapsed = 0f;
        
        //ȸ��
        while (timeElapsed < rotationDuration)
        {
            ovenDoor.localRotation = Quaternion.Slerp
                (startRotation, endRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;  // �� ������ ���
        }

        ovenDoor.localRotation = endRotation;  // ���� ȸ���� ����
        isRotating = false;  // ȸ�� �Ϸ�
        isOpen = !isOpen;  // ���� ������ ȸ�� �Ϸ� �Ŀ� ����
    }
}