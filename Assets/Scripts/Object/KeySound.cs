using UnityEngine;

public class KeySound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("���� �浹��");
        SoundManager.instance.SFXPlay("DropKey_SFX", this.gameObject);
    }
}