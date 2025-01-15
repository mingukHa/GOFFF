using UnityEngine;

public class ValveSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("º§ºê Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("DropItem_SFX", this.gameObject);
    }
}