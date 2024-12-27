using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    public delegate void CollisionEvent(Vector3 collisionPoint); //�ݹ� ��������Ʈ
    public static event CollisionEvent OnObjectHitGround; //�浹 �̺�Ʈ

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object")) //���� �浹�Ѱ� ������Ʈ���
        {
            Vector2 collisionPoint = collision.contacts[0].point; //�浹 �� ������Ʈ�� ��ǥ�� �����´�
            Debug.Log($"��ǥ ��ġ{collision.contacts[0].point}"); //��ǥ Ȯ�� ��

            OnObjectHitGround?.Invoke(collisionPoint); //�ݹ��� ȣ���Ѵ�

        }
    }
}
