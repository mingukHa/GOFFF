using UnityEngine;

public class LoginCameraMove : MonoBehaviour //코그인 카메라 회전
{
    public float rotationSpeed = 10f; 

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        //로그인씬 카메라 화면의 회전
    }
}


