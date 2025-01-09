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
        // ��ư�� ���ȴ��� Ȯ��
        if (isButtonPressed)
        {
            // 2�� �ڽ�(ovenbottom[0])�� ��ġ���� �浹�� ��� ������Ʈ ã��
            Collider[] colliders = Physics.OverlapBox(
                ovenbottom[0].position,
                ovenbottom[0].localScale / 2,
                Quaternion.identity
            );

            foreach (Collider col in colliders)
            {
                // �浹�� ������Ʈ�� ��ư�� �ƴ� ���
                if (col.gameObject != telpobtn)
                {
                    // ������Ʈ�� ovenbottom[1] ��ġ�� �̵�
                    col.transform.position = ovenbottom[1].position;
                }
            }

            yield return null;
        }
    }

    // ��ư�� ������ �� isButtonPressed�� true�� ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == telpobtn) // ��ư�� ������
        {
            isButtonPressed = true;
        }
    }

    // ��ư�� �������� �� isButtonPressed�� false�� ����
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == telpobtn)
        {
            isButtonPressed = false;
        }
    }
}