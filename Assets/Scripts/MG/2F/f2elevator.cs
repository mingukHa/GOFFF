using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class f2elevators : MonoBehaviourPun
{
    [SerializeField] private GameObject button1;
    [SerializeField] private Transform targetPos;

    

    private void OnMoveButtonPressed()
    {
        // ��ư�� ���� �÷��̾ �ڽ����� Ȯ��
        if (photonView.IsMine)
        {
            photonView.RPC("MoveToTarget", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void MoveToTarget(int actorNumber)
    {
        // ��Ʈ��ũ���� Ư�� �÷��̾� �ĺ�
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            // ������ ��ġ�� �̵�
            transform.position = targetPos.position;
            PhotonNetwork.Destroy(button1);

        }
    }

}