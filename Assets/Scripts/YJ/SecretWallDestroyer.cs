using UnityEngine;

public class SecretWallDestroyer : MonoBehaviour
{
    private bool isCollision = false;
    private float currentRotationX = 0f; // 현재까지의 누적 회전값

    private void Update()
    {
        if (isCollision && currentRotationX < 90f)
        {
            // 누적 회전값을 계산
            float rotationStep = Time.deltaTime * 90f; // 1초에 90도 회전
            float rotationToAdd = Mathf.Min(rotationStep, 90f - currentRotationX); // 90도를 넘지 않도록 제한
            currentRotationX += rotationToAdd;

            // 객체의 회전에 누적
            transform.rotation *= Quaternion.Euler(new Vector3(rotationToAdd, 0, 0));
        }
        else if (isCollision && currentRotationX >= 90f)
        {
            // 90도 회전 후, 오브젝트를 파괴
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
