using UnityEngine;

public class CanSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        //Debug.Log("ĵ �浹��");
        SoundManager.instance.SFXPlay("CanThrow_SFX", this.gameObject);
    }
}