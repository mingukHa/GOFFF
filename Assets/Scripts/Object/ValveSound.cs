using UnityEngine;

public class ValveSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("���� �浹��");
        SoundManager.instance.SFXPlay("AttachValve_SFX", this.gameObject);
    }
}