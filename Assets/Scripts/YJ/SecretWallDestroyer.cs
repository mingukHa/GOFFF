using UnityEngine;

public class SecretWallDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체에 PlayerController 스크립트가 있는지 확인
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // finalMovement.magnitude 값을 가져오기 위해 PlayerController 속성 사용
            float movementMagnitude = playerController.photonView.IsMine
                ? playerController.GetCurrentMovementMagnitude()
                : 0f;

            Debug.Log("현재 속도: " + movementMagnitude);

            // finalMovement.magnitude가 4 이상일 때 객체 파괴
            if (movementMagnitude >= 4f)
            {
                Destroy(gameObject); // SecretWallDestroyer 객체 파괴
            }
        }
    }
}
