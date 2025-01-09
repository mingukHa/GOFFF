using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Invoke(nameof(SendRestartRPC), 2f); // 2초 뒤 RPC 호출
        }
    }

    private void SendRestartRPC()
    {
        // 모든 클라이언트에 씬 리로드 요청
        photonView.RPC("ReStart", RpcTarget.Others);
    }

    [PunRPC]
    private void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string SceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            PhotonNetwork.LoadLevel(SceneName);
        }
        else
        {
            Debug.Log("씬 로드는 마스터 클라이언트에서 처리됩니다.");
        }
    }
}
