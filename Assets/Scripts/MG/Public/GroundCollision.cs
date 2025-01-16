using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    public delegate void CollisionEvent(Vector3 collisionPoint);
    public static event CollisionEvent OnObjectHitGround;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object")) //���࿡ �ٴ��� ��ü�� ������Ʈ���
        {
            Vector3 collisionPoint = collision.contacts[0].point; // �浹 ��ǥ
            //Debug.Log($"GroundCollision���� ���޵� ��ǥ: {collisionPoint}"); // ���ް� �����
            OnObjectHitGround?.Invoke(collisionPoint); // �̺�Ʈ ȣ��
        }
    }

}
