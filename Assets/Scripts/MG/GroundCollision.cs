using UnityEngine;

public class GroundCollision : MonoBehaviour
{
    public delegate void CollisionEvent(Vector3 collisionPoint);
    public static event CollisionEvent OnObjectHitGround;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            Vector3 collisionPoint = collision.contacts[0].point; // 충돌 좌표
            Debug.Log($"GroundCollision에서 전달된 좌표: {collisionPoint}"); // 전달값 디버깅
            OnObjectHitGround?.Invoke(collisionPoint); // 이벤트 호출
            SoundManager.instance.SFXPlay("CanThrow_SFX");
        }
    }

}
