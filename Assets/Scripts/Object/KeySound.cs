using UnityEngine;

public class KeySound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("¿­¼è Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("DropKey_SFX", this.gameObject);
    }
}