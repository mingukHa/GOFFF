using UnityEngine;
using Photon.Pun;

public class PlayerDead : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster")) //콜라이더가 몬스터에 닿으며
        {
            photonView.RPC(nameof(NotifyDeath), RpcTarget.All); //모두에게 RPC를 쏜다
        }
    }

    [PunRPC]
    public void NotifyDeath()
    {
        if (PhotonNetwork.IsMasterClient)  //마스터 클라이어트면
        {
            Invoke(nameof(RestartLevel), 2f); //2초 뒤 함수 실행
        }
    }

    private void RestartLevel()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; //현재 씬 이름을 저장하고
        PhotonNetwork.LoadLevel(sceneName); //씬을 불러온다
    }
}
