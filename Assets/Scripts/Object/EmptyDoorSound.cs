using UnityEngine;

public class EmptyDoorSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("문 잠겨있음");
        SoundManager.instance.SFXPlay("LockDoor_SFX", this.gameObject);
    }
}