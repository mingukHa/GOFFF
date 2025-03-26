using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class F2Elevators : MonoBehaviourPun //2√˛ ø§∏Æ∫£¿Ã≈Õ
{
    [SerializeField] private GameObject button1;
    [SerializeField] private Transform targetPos;
    [SerializeField] private GameObject[] player;

    public void OnMoveButtonPressed()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("MoveToTarget", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void MoveToTarget(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            player[PhotonNetwork.LocalPlayer.ActorNumber-1].gameObject.transform.position = targetPos.position;

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(button1);
            }
        }
    }
}
