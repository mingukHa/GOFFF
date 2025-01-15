using UnityEngine;

public class SecretWallDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� PlayerController ��ũ��Ʈ�� �ִ��� Ȯ��
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // finalMovement.magnitude ���� �������� ���� PlayerController �Ӽ� ���
            float movementMagnitude = playerController.photonView.IsMine
                ? playerController.GetCurrentMovementMagnitude()
                : 0f;

            Debug.Log("���� �ӵ�: " + movementMagnitude);

            // finalMovement.magnitude�� 4 �̻��� �� ��ü �ı�
            if (movementMagnitude >= 4f)
            {
                Destroy(gameObject); // SecretWallDestroyer ��ü �ı�
            }
        }
    }
}
