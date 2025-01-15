using UnityEngine;

public class BurgerSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("¹°°Ç Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("DropItem_SFX", this.gameObject);
    }
}