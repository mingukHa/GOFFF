using Photon.Pun;
using UnityEngine;

public class GameOverManagers : MonoBehaviourPun
{
    private GameObject Player1; // PlayerHolder
    private GameObject Player2; // PlayerHolder1

    [SerializeField] private Transform spwan1;
    [SerializeField] private Transform spwan2;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ReStart();
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }
    private void Awake()
    {
        // 이름으로 플레이어 오브젝트 찾기
        Player1 = GameObject.Find("PlayerHolder(Clone)");
        Player2 = GameObject.Find("PlayerHolder1(Clone)");
        Debug.Log($"{Player1},{Player2} 확인이 됨");  
        if (Player1 == null)
        {
            Debug.LogError($"{Player1}확인이 안 됨");
        }
        if (Player2 == null)
        {
            Debug.LogError($"{Player2}확인이 안 됨");
        }
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 플레이어 위치 초기화
            if (Player1 != null && Player2 != null)
            {
                Player1.transform.position = spwan1.position;
                Debug.Log($"{Player1.transform.position}, {spwan1.position}");
                Player2.transform.position = spwan2.position;
                Debug.Log($"{Player2.transform.position}, {spwan2.position}");
            }
            else
            {
                Debug.LogError("Player1 또는 Player2가 null입니다. 위치 초기화를 진행할 수 없습니다.");
            }

        }
    }
}
