using UnityEngine;
using System.Collections;

public class Domwaiter : MonoBehaviour
{
    public Transform ovenDoor;  // ���� �� ������Ʈ
    private bool isOpen = false;
    private bool isRotating = false; // ȸ�� ���̸� �߰� Ʈ���Ÿ� ����
    public float rotationDuration = 1f;  // ���� ȸ���ϴ� �ð� (�ν����Ϳ��� ���� ����)

    // �� ���� ���� �Լ�
    public void ToggleDoor()
    {
        if (isRotating) return;  // ���� ȸ�� ���̸� ������� ����
        StartCoroutine(RotateDoor(isOpen ? 0f : 90f));  // ���� ������ �ݱ�, �ƴϸ� ����
    }

    // �� ȸ�� �ڷ�ƾ
    private IEnumerator RotateDoor(float targetRotation)
    {
        isRotating = true;

        Quaternion startRotation = ovenDoor.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, targetRotation, 0f);
        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            ovenDoor.localRotation = Quaternion.Slerp(startRotation, endRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;  // �� ������ ���
        }

        ovenDoor.localRotation = endRotation;  // ���� ȸ���� ����
        isRotating = false;  // ȸ�� �Ϸ�
        isOpen = !isOpen;  // ���� ������ ȸ�� �Ϸ� �Ŀ� ����
    }
}
