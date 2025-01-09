using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class f4elevators : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    public string Scene = "MainScenes";
    private int readyPlayerCount = 0; // �غ� �Ϸ�� �÷��̾� ��

    public void OnButtonPressed()
    {
        //if (PhotonNetwork.IsMasterClient)
       // {
            photonView.RPC("PlayerReady", RpcTarget.AllBuffered); // ��� Ŭ���̾�Ʈ�� �÷��̾� �غ� ���� ����
            Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

            // ��� �÷��̾ �غ�Ǿ��� ��� ���� ������ ��ȯ
            if (readyPlayerCount == 2)
            {

                Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
                PhotonNetwork.LoadLevel(Scene); // ��ȯ�� �� �̸����� ����

            }
            else
            {
                return;
            }
      //  }
        Debug.Log("��ư�� ���Ƚ��ϴ�");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]
    public void PlayerReady()
    {
        readyPlayerCount++;
    }
}