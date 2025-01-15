using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class f3elevators : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject button1; //버튼1
    [SerializeField] private GameObject button2; //버튼2
    public string Scene = "MainScenes";
    private int readyPlayerCount = 0; // 준비 완료된 플레이어 수

    public void OnButtonPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("PlayerReady1", RpcTarget.AllBuffered); // 모든 클라이언트에 플레이어 준비 상태 전달
            //Debug.Log($"현재 준비된 플레이어 수: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");

            // 모든 플레이어가 준비되었을 경우 다음 씬으로 전환
            if (readyPlayerCount == 2)
            {

            //Debug.Log("2명 준비 완료! 다음 씬으로 이동합니다.");
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel(Scene); // 전환할 씬 이름으로 변경
                }
            }
            else
            {
                return;
            }
        }
        Debug.Log("버튼이 눌렸습니다");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]
    public void PlayerReady1()
    {
        readyPlayerCount++; //플레이 레디 카운트 증가
    }
}