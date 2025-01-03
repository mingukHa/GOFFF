using UnityEngine;

public class RespawnPointCreator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                Vector3 newRespawnPoint = transform.position;
                RespawnManager.Instance.UpdateRespawnPoint(player.photonView.OwnerActorNr, newRespawnPoint);
            }
        }
    }
}
