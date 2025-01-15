using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotationSpeed = 10f; 

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
    }
}


