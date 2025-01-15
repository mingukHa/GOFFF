using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class f3elevators : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject button1; //��ư1
    [SerializeField] private GameObject button2; //��ư2
    public string Scene = "MainScenes";
    private int readyPlayerCount = 0; // �غ� �Ϸ�� �÷��̾� ��

    public void OnButtonPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerReady1", RpcTarget.AllBuffered); // ��� Ŭ���̾�Ʈ�� �÷��̾� �غ� ���� ����
            //Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

            // ��� �÷��̾ �غ�Ǿ��� ��� ���� ������ ��ȯ
            if (readyPlayerCount == 2)
            {

            //Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(Scene); // ��ȯ�� �� �̸����� ����
                }
            }
            else
            {
                return;
            }
        }
        Debug.Log("��ư�� ���Ƚ��ϴ�");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]
    public void PlayerReady1()
    {
        readyPlayerCount++; //�÷��� ���� ī��Ʈ ����
    }
}