using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    public delegate void CollisionEvent(Vector3 collisionPoint); //콜백 델리게이트
    public static event CollisionEvent OnObjectHitGround; //충돌 이벤트

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object")) //만약 충돌한게 오브젝트라면
        {
            Vector2 collisionPoint = collision.contacts[0].point; //충돌 한 오브젝트의 좌표를 가져온다
            Debug.Log($"좌표 위치{collision.contacts[0].point}"); //좌표 확인 용

            OnObjectHitGround?.Invoke(collisionPoint); //콜백을 호출한다

        }
    }
}
