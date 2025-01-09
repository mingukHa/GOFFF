using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    public Transform[] ovenbottom;

    private bool isButtonPressed = false;

    public void OnButtonPressed()
    {
        isButtonPressed = true;
        telpo();
    }

    // XR Simple Interactable�� Select Exit �̺�Ʈ�� ������ �޼���
    public void OnButtonReleased()
    {
        isButtonPressed = false;
    }

    public void telpo()
    {
        StartCoroutine(Telpoburgur());
    }

    private IEnumerator Telpoburgur()
    {
        if (isButtonPressed)
        {
            // ovenbottom[0]�� �浹 �ڽ� ũ�⸦ ����
            Vector3 boxSize = new Vector3(1f, 1f, 1f); // ���ϴ� ũ��� ����

            // ovenbottom[0]�� �ڽ� �浹ü �ȿ� �ִ� ��� ������Ʈ �˻�
            Collider[] colliders = Physics.OverlapBox(ovenbottom[0].position, boxSize / 2);

            // �˻��� ��� ������Ʈ�� ovenbottom[1] ��ġ�� �̵�
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Food")) // Ư�� �±��� ������Ʈ�� �̵�
                {
                    col.transform.position = ovenbottom[1].position;
                }
            }

            yield return null;
        }
    }

    // �浹 �ڽ��� �ð�ȭ�Ͽ� Ȯ���� �� �ֵ��� OnDrawGizmos �߰�
    private void OnDrawGizmos()
    {
        if (ovenbottom != null && ovenbottom.Length > 0)
        {
            Gizmos.color = Color.red;
            Vector3 boxSize = new Vector3(1f, 1f, 1f);
            Gizmos.DrawWireCube(ovenbottom[0].position, boxSize);
        }
    }
}