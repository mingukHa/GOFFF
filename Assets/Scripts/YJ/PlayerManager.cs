using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            // �÷��̾��� �ʱ� ��ġ�� SaveManager�� ���
            SaveManager.Instance.RegisterSavePoint(photonView.OwnerActorNr, transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        // Debug�� Self Destroy �޼ҵ�
        if (Input.GetKeyUp(KeyCode.R))
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        if (!photonView.IsMine) return;

        RespawnManager.Instance.HandlePlayerRespawn(gameObject, photonView);
    }

    [PunRPC]
    public void SetPlayerActive(bool isActive)
    {
        if (gameObject.activeSelf == isActive) return;
        gameObject.SetActive(isActive);
    }

    [PunRPC]
    public void RespawnPlayer(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }
}
