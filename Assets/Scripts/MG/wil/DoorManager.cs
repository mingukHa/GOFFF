using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false; // �� ���� (����/����)
    public float openAngle = -90f; // ���� ���� ���� (��: 90�� �Ǵ� -90��)
    public float animationTime = 1f; // �� ����/���� �ִϸ��̼� �ð�

    private Quaternion closedRotation; // ���� ������ ȸ����
    private Quaternion openRotation; // ���� ������ ȸ����

    void Start()
    {
        // �ʱ� ȸ���� ����
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    // �� ����/�ݱ� ���
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
            Debug.Log("����");
        }
        else
        {
            OpenDoor();
            Debug.Log("����");
        }
    }

    // �� ����
    public void OpenDoor()
    {
        SoundManager.instance.SFXPlay("OpenDoor2_SFX");
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(openRotation));
        isOpen = true;
    }

    // �� �ݱ�
    private void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(closedRotation));
        isOpen = false;
    }

    // �� �ִϸ��̼� ó��
    private System.Collections.IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < animationTime)
        {
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
