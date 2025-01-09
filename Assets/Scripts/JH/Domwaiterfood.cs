using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    public Transform[] ovenbottom;
    public GameObject telpobtn;

    private bool isButtonPressed = false;

    public void telpo()
    {
        StartCoroutine(Telpoburgur());
    }

    private IEnumerator Telpoburgur()
    {
        // ��ư�� ���ȴ��� Ȯ�� (���⼭�� isButtonPressed ������ ���¸� ����)
        if (isButtonPressed)
        {
            // 2�� �ڽ�(ovenbottom[0])�� �浹�� ��� ������Ʈ�� ã��
            Collider[] colliders = Physics.OverlapBox(ovenbottom[0].position, ovenbottom[0].localScale / 2);

            foreach (Collider col in colliders)
            {
                // �浹�� ������Ʈ�� ��ư�� �ƴ� �ٸ� ������Ʈ���
                if (col.gameObject != telpobtn)
                {
                    // ovenbottom[1] ��ġ�� ������Ʈ �̵�
                    col.transform.position = ovenbottom[1].position;
                }
            }

            // �ڷ�ƾ ����
            yield return null;
        }
    }

    // ��ư�� ������ �� isButtonPressed�� true�� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == telpobtn) // ��ư�� ������
        {
            isButtonPressed = true;
        }
    }

    // ��ư�� �������� �� isButtonPressed�� false�� ����
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == telpobtn)
        {
            isButtonPressed = false;
        }
    }
}