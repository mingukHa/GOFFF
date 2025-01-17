using UnityEngine;

public class EmptyDoorSound : MonoBehaviour
{
    public void CloseDoor()
    {
        SoundManager.instance.SFXPlay("LockDoor_SFX", this.gameObject);
    }
}