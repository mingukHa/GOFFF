using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class f4elevators : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    public string Scene = "MainScenes"; //�� �̸��� �ν����Ϳ��� ���� ����
    private int readyPlayerCount = 0; // �غ� �Ϸ�� �÷��̾� ��

    public void OnButtonPressed()
    {
        
            photonView.RPC("PlayerReady1", RpcTarget.AllBuffered); // ��� Ŭ���̾�Ʈ�� �÷��̾� �غ� ���� ����
            Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}"); //�غ� �ο� / ���� �� ���� �ο�

            // ��� �÷��̾ �غ�Ǿ��� ��� ���� ������ ��ȯ
            if (readyPlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
                PhotonNetwork.LoadLevel(Scene); // ��ȯ�� �� �̸����� ����
                }
            }
            else
            {
                return;
            }
        
        Debug.Log("��ư�� ���Ƚ��ϴ�");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]//���� ������ �Լ� rpc ���
    public void PlayerReady1() 
    {
        readyPlayerCount++;
    }
}