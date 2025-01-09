using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private PlayerDead pd;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            pd.Deadzone();
        }
    }
}
