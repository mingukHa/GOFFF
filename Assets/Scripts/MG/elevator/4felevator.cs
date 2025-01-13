using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class f4elevators : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    public string Scene = "MainScenes"; //씬 이름은 인스펙터에서 수정 가능
    private int readyPlayerCount = 0; // 준비 완료된 플레이어 수

    public void OnButtonPressed()
    {
        
            photonView.RPC("PlayerReady1", RpcTarget.AllBuffered); // 모든 클라이언트에 플레이어 준비 상태 전달
            Debug.Log($"현재 준비된 플레이어 수: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}"); //준비 인원 / 현재 방 참여 인원

            // 모든 플레이어가 준비되었을 경우 다음 씬으로 전환
            if (readyPlayerCount == 2)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                Debug.Log("2명 준비 완료! 다음 씬으로 이동합니다.");
                PhotonNetwork.LoadLevel(Scene); // 전환할 씬 이름으로 변경
                }
            }
            else
            {
                return;
            }
        
        Debug.Log("버튼이 눌렸습니다");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]//레디 누르는 함수 rpc 등록
    public void PlayerReady1() 
    {
        readyPlayerCount++;
    }
}