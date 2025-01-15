using UnityEngine;

public class SecretWallDestroyer : MonoBehaviour
{
    private bool isCollision = false;
    private float newRotationX;

    private void Update()
    {
        if (isCollision)
        {
            newRotationX = Mathf.Lerp(0, 90f, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Euler(new Vector3(newRotationX, transform.rotation.y, transform.rotation.z));
            if (transform.position.x < 1f)
            {
                Invoke("DestroygameObject", 2f);
            }
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
