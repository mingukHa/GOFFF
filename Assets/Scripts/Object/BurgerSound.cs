using UnityEngine;

public class BurgerSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("���� �浹��");
        SoundManager.instance.SFXPlay("DropItem_SFX", this.gameObject);
    }
}