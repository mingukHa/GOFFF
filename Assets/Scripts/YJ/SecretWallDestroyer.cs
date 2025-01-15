using UnityEngine;

public class SecretWallDestroyer : MonoBehaviour
{
    private bool isCollision = false;
    private float currentRotationX = 0f; // ��������� ���� ȸ����

    private void Update()
    {
        if (isCollision && currentRotationX < 90f)
        {
            // ���� ȸ������ ���
            float rotationStep = Time.deltaTime * 90f; // 1�ʿ� 90�� ȸ��
            float rotationToAdd = Mathf.Min(rotationStep, 90f - currentRotationX); // 90���� ���� �ʵ��� ����
            currentRotationX += rotationToAdd;

            // ��ü�� ȸ���� ����
            transform.rotation *= Quaternion.Euler(new Vector3(rotationToAdd, 0, 0));
        }
        else if (isCollision && currentRotationX >= 90f)
        {
            // 90�� ȸ�� ��, ������Ʈ�� �ı�
            Invoke(nameof(DestroygameObject), 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isCollision = true;
    }

    private void DestroygameObject()
    {
        Destroy(gameObject);
    }
}
