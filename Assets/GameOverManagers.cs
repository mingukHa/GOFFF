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
            Debug.Log("Space Ű �Է� ������. ReStart ȣ�� ��...");
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

        if (Player1 == null) Debug.LogError("Player1�� ã�� �� �����ϴ�.");
        if (Player2 == null) Debug.LogError("Player2�� ã�� �� �����ϴ�.");
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Player1 != null && Player2 != null)
            {
                // ������ Ŭ���̾�Ʈ���� ���� ��ġ�� ��ε�ĳ��Ʈ
                Debug.Log("ReStart ȣ���. ��ġ�� ������Ʈ ��...");
                photonView.RPC("UpdatePlayerPosition", RpcTarget.All, spwan1.position, spwan2.position , monsterspawn.position);
            }
            else
            {
                Debug.LogError("Player1 �Ǵ� Player2�� null�Դϴ�.");
            }
        }
    }

    [PunRPC]
    private void UpdatePlayerPosition(Vector3 position1, Vector3 position2, Vector3 positon3)
    {
        
        // ��� Ŭ���̾�Ʈ���� ��ġ�� ������Ʈ
        if (Player1 != null)
        {
            Player1.transform.position = position1;
            Debug.Log($"Player1 ��ġ ������Ʈ: {position1}");
        }
        if (Player2 != null)
        {
            Player2.transform.position = position2;
            Debug.Log($"Player2 ��ġ ������Ʈ: {position2}");
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
//        // �̸����� �÷��̾� ������Ʈ ã��
//        Player1 = GameObject.Find("PlayerHolder(Clone)");
//        Player2 = GameObject.Find("PlayerHolder1(Clone)");
//        Debug.Log($"{Player1},{Player2} Ȯ���� ��");  
//        if (Player1 == null)
//        {
//            Debug.LogError($"{Player1}Ȯ���� �� ��");
//        }
//        if (Player2 == null)
//        {
//            Debug.LogError($"{Player2}Ȯ���� �� ��");
//        }
//    }

//    [PunRPC]
//    public void ReStart()
//    {
//        if (PhotonNetwork.IsMasterClient)
//        {
//            // �÷��̾� ��ġ �ʱ�ȭ
//            if (Player1 != null && Player2 != null)
//            {
//                //Player1.transform.position = spwan1.position;
//                Debug.Log($"{Player1.transform.position}, {spwan1.position}");
//                Player2.transform.position = spwan2.position;
//                Debug.Log($"{Player2.transform.position}, {spwan2.position}");
//            }
//            else
//            {
//                Debug.LogError("Player1 �Ǵ� Player2�� null�Դϴ�. ��ġ �ʱ�ȭ�� ������ �� �����ϴ�.");
//            }

//        }
//    }
//}
