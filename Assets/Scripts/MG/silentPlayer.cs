using UnityEngine;

public class slientPlayer : MonoBehaviour
{
    public CharacterController characterController;
    public float moveSpeed = 6f; // �̵� �ӵ�
    public float rotateSpeed = 180f; // ȸ�� �ӵ�
    public float jumpHeight = 2f; // ���� ����
    public float gravity = -9.81f; // �߷� ��
    public GameObject gameObject1;
    private Vector3 velocity; // ������ �߷� �ӵ� ����
    private bool isGrounded; // �ٴڿ� �ִ��� ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            gameObject1.SetActive(true);
        // �ٴ� ����
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // �ٴڿ� ���� �� �߷� �ʱ�ȭ
        }

        // �̵� �� ȸ��
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        // ����/���� �̵�
        Vector3 move = transform.forward * axisV * moveSpeed;
        characterController.SimpleMove(move); // SimpleMove�� �̵����� ��� (�߷��� ������ ó��)

        // ȸ��
        transform.Rotate(0, axisH * rotateSpeed * Time.deltaTime, 0);

        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // ���� �ӵ� ���
        }

        // �߷� ����
        velocity.y += gravity * Time.deltaTime;

        // ĳ���� ��Ʈ�ѷ��� �߷� �ӵ� �ݿ�
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�浹�� ������");
    }


}

