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
        // 버튼을 누른 플레이어가 자신인지 확인
        if (photonView.IsMine)
        {
            photonView.RPC("MoveToTarget", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void MoveToTarget(int actorNumber)
    {
        // 네트워크에서 특정 플레이어 식별
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            // 지정된 위치로 이동
            transform.position = targetPos.position;
            PhotonNetwork.Destroy(button1);

        }
    }

}