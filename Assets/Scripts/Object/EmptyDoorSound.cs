using UnityEngine;

public class EmptyDoorSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("�� �������");
        SoundManager.instance.SFXPlay("LockDoor_SFX", this.gameObject);
    }
}