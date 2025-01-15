using UnityEngine;

public class BookSound : MonoBehaviour
{
    public void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("Ã¥ Ãæµ¹µÊ");
        SoundManager.instance.SFXPlay("Book_SFX", this.gameObject);
    }
}