using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class f2elevators : MonoBehaviourPun
{
    [SerializeField] private GameObject button1;
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameObject player1;
    

    public void OnMoveButtonPressed()
    {
        // ��ư�� ���� �÷��̾ �ڽ����� Ȯ��
        if (photonView.IsMine)
        {
            photonView.RPC("MoveToTarget", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void MoveToTarget()
    {
        // ��Ʈ��ũ���� Ư�� �÷��̾� �ĺ�
        
            // ������ ��ġ�� �̵�
            player1.gameObject.transform.position = targetPos.position;
            PhotonNetwork.Destroy(button1);

        
    }

}