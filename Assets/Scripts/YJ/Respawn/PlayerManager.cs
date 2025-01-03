using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    private bool isDisabled = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // ó�� ���� �� �ʱ� Respawn Point ���
            RespawnManager.Instance.RegisterInitialRespawnPoint(photonView.OwnerActorNr, transform.position);
            Debug.Log("�ʱ� ������ ����Ʈ ���� �Ϸ�");
        }
    }

    private void Update()
    {
        // Debug�� Instant Death
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            RespawnManager.Instance.TriggerGameOver();
        }
    }

    public void DisablePlayer()
    {
        if (isDisabled) return;

        isDisabled = true;
        gameObject.SetActive(false); // Player ��Ȱ��ȭ
    }

    public void RespawnAt(Vector3 respawnPoint)
    {
        transform.position = respawnPoint;
        gameObject.SetActive(true); // Player Ȱ��ȭ
        isDisabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ư�� ��Ҹ� �������� Respawn Point ����
        if (other.CompareTag("RespawnPoint") && photonView.IsMine)
        {
            RespawnManager.Instance.UpdateRespawnPoint(photonView.OwnerActorNr, transform.position);
        }
    }
}
