using UnityEngine;

public class LoginCameraMove : MonoBehaviour //�ڱ��� ī�޶� ȸ��
{
    public float rotationSpeed = 10f; 

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        //�α��ξ� ī�޶� ȭ���� ȸ��
    }
}


