using UnityEngine;

public class BookSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("å �浹��");
        SoundManager.instance.SFXPlay("Book_SFX", this.gameObject);
    }
}