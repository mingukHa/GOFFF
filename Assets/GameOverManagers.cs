using Photon.Pun;
using UnityEngine;

public class GameOverManagers : MonoBehaviourPun
{
    private GameObject Player1;
    private GameObject Player2;

    [SerializeField] private Transform spwan1;
    [SerializeField] private Transform spwan2;
    [SerializeField] private Transform monsterspawn;
    [SerializeField] private GameObject monster;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space 키 입력 감지됨. ReStart 호출 중...");
            photonView.RPC("ReStart", RpcTarget.All);
        }
    }

    private void Awake()
    {
        foreach (var player in FindObjectsOfType<PhotonView>())
        {
            if (player.IsMine && player.gameObject.name.Contains("PlayerHolder"))
            {
                Player1 = player.gameObject;
            }
            else if (!player.IsMine && player.gameObject.name.Contains("PlayerHolder"))
            {
                Player2 = player.gameObject;
            }
        }

        if (Player1 == null) Debug.LogError("Player1을 찾을 수 없습니다.");
        if (Player2 == null) Debug.LogError("Player2를 찾을 수 없습니다.");
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Player1 != null && Player2 != null)
            {
                // 마스터 클라이언트에서 스폰 위치를 브로드캐스트
                Debug.Log("ReStart 호출됨. 위치를 업데이트 중...");
                photonView.RPC("UpdatePlayerPosition", RpcTarget.All, spwan1.position, spwan2.position , monsterspawn.position);
            }
            else
            {
                Debug.LogError("Player1 또는 Player2가 null입니다.");
            }
        }
    }

    [PunRPC]
    private void UpdatePlayerPosition(Vector3 position1, Vector3 position2, Vector3 positon3)
    {
        
        // 모든 클라이언트에서 위치를 업데이트
        if (Player1 != null)
        {
            Player1.transform.position = position1;
            Debug.Log($"Player1 위치 업데이트: {position1}");
        }
        if (Player2 != null)
        {
            Player2.transform.position = position2;
            Debug.Log($"Player2 위치 업데이트: {position2}");
        }
        if (monster != null)
        {
            monster.transform.position = positon3;
        }
    }
}

//using Photon.Pun;
//using UnityEngine;

//public class GameOverManagers : MonoBehaviourPun
//{
//    private GameObject Player1; // PlayerHolder
//    private GameObject Player2; // PlayerHolder1

//    [SerializeField] private Transform spwan1;
//    [SerializeField] private Transform spwan2;
//    private void FixedUpdate()
//    {
//        if (Input.GetKey(KeyCode.Space))
//        {
//            ReStart();
//            photonView.RPC("ReStart", RpcTarget.All);
//        }
//    }
//    private void Awake()
//    {
//        // 이름으로 플레이어 오브젝트 찾기
//        Player1 = GameObject.Find("PlayerHolder(Clone)");
//        Player2 = GameObject.Find("PlayerHolder1(Clone)");
//        Debug.Log($"{Player1},{Player2} 확인이 됨");  
//        if (Player1 == null)
//        {
//            Debug.LogError($"{Player1}확인이 안 됨");
//        }
//        if (Player2 == null)
//        {
//            Debug.LogError($"{Player2}확인이 안 됨");
//        }
//    }

//    [PunRPC]
//    public void ReStart()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            // 플레이어 위치 초기화
//            if (Player1 != null && Player2 != null)
//            {
//                //Player1.transform.position = spwan1.position;
//                Debug.Log($"{Player1.transform.position}, {spwan1.position}");
//                Player2.transform.position = spwan2.position;
//                Debug.Log($"{Player2.transform.position}, {spwan2.position}");
//            }
//            else
//            {
//                Debug.LogError("Player1 또는 Player2가 null입니다. 위치 초기화를 진행할 수 없습니다.");
//            }

//        }
//    }
//}
