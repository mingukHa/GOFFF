using UnityEngine;
using Photon.Pun;

public class TVScreenToggle : MonoBehaviourPun
{
    public GameObject screen1; // Screen 1
    public GameObject screen2; // Screen 2
    private bool isScreen1Active = true; // Screen 1의 초기 활성 상태

    private void Start()
    {
        // 초기 상태 설정
        UpdateScreen(isScreen1Active);
    }

    public void ToggleScreen()
    {
        if (PhotonNetwork.IsConnected)
        {
            // RPC 호출로 모든 플레이어에게 TV 상태 동기화
            photonView.RPC("ToggleScreenRPC", RpcTarget.AllBuffered);
        }
        else
        {
            // 네트워크가 아닌 로컬에서만 동작
            UpdateScreen(!isScreen1Active);
        }
    }

    [PunRPC]
    private void ToggleScreenRPC()
    {
        // 활성 상태 전환 및 화면 업데이트
        isScreen1Active = !isScreen1Active;
        UpdateScreen(isScreen1Active);
    }

    private void UpdateScreen(bool showScreen1)
    {
        // 화면 활성화/비활성화 설정
        screen1.SetActive(showScreen1);
        screen2.SetActive(!showScreen1);
    }
}
