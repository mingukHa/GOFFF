using UnityEngine;

public class CanSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        //Debug.Log("Äµ Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("CanThrow_SFX", this.gameObject);
    }
}