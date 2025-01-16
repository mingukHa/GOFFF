using UnityEngine;

public class ValveSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("º§ºê Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("AttachValve_SFX", this.gameObject);
    }
}