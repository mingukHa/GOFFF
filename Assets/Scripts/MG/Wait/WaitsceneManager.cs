using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class Waitscene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; //�÷��̾� �������� �޾� �� �迭
    [SerializeField] private Transform[] spawnPoints; //�÷��̾��� ���� ��ġ
    [SerializeField] private GameObject button1; //���������� ��ư1
    [SerializeField] private GameObject button2; //���������� ��ư 2
    public string Scene = "MainScenes"; //�ε� �� ���� �̸��� �ν����ͷ� ���� ����
    private bool hasSpawned = false; //���� ����
    private int readyPlayerCount = 0; // �غ� �Ϸ�� �÷��̾� ��

    private void Start()
    {
        if (!PhotonNetwork.IsConnected) //���� ��Ʈ��ũ�� ������ �� �Ǿ� �ִٸ�
        {
            Debug.Log("���� ������ ������ �� �Ǿ��� �κ�� �̵�");
            SceneManager.LoadScene("LoginScenes"); //�α��� ������ �ٽ� �̵�
            return;
        }
        if (!PhotonNetwork.InRoom) //�濡 ������ �� �ߴٸ�
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�. �� ������ ��ٸ��ϴ�...");
            return; 
        }
        StartCoroutine(WaitForRoomReady()); //�� ���� ��� �ڷ�ƾ ����
    }

    public override void OnJoinedRoom() //�濡 ������
    {
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned) //������ �� �Ǿ��ٸ�
        {
            SpawnPlayer(); //����
        }
    }

    private IEnumerator WaitForRoomReady() //���� �� ���� ��� �ڷ�ƾ
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom); //����,�濡 ���Դٸ�
        SpawnPlayer(); //�÷��̾� ����
    }

    private void SpawnPlayer() //���� �Լ�
    {
        if (playerPrefab == null) //�������� ������
        {
            Debug.LogError("Player Prefab�� �������� �ʾҽ��ϴ�!");
            return; //���ư�
        }

        if (spawnPoints == null || spawnPoints.Length == 0) //���� �迭�� ���ٸ� ���ư�
        {
            Debug.LogError("���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return; 
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //��Ʈ�ѹ��� 1���� ����, �迭�� 0���� ����
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length]; //��Ʈ�ѹ��� �´� �迭��ġ�� ����

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);
        //�÷��̾� ��Ʈ �ѹ��� �´� �÷��̾� ������ ����
        
        hasSpawned = true;
        //���� �� Ʈ��

        StartCoroutine(ReenableCollider(player));

        // ��� Ŭ���̾�Ʈ�� DontDestroyOnLoad ����
        photonView.RPC("SetDontDestroyOnLoad", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }
    //���� �Ѿ�� ��Ʈ�ѷ��� �ڲ� ����ȭ ������ ���ϰ� ����. �׷��Ƿ� �� ��Ʈ���̷� ����
    [PunRPC]
    private void SetDontDestroyOnLoad(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null && targetView.gameObject != null)
        {
            DontDestroyOnLoad(targetView.gameObject);
            Debug.Log("��� PlayerHolder�� DontDestroyOnLoad ���� �Ϸ�.");
        }
    }
    //�����ϰ� �ݶ��̴��� �ε����� �� �ָ� ���ư�. 1���� ���ð� �ֱ�
    private IEnumerator ReenableCollider(GameObject player)
    {
        if (player == null) yield break;

        Collider collider = player.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
            yield return new WaitForSeconds(1);
            collider.enabled = true;
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnButtonPressed()
    {
        if (PhotonNetwork.IsMasterClient) //�����̶��
        {
            photonView.RPC("PlayerReady", RpcTarget.AllBuffered); // ��� Ŭ���̾�Ʈ�� �÷��̾� �غ� ���� ����
            Debug.Log($"���� �غ�� �÷��̾� ��: {readyPlayerCount}/{PhotonNetwork.CurrentRoom.PlayerCount}");
            
            // ��� �÷��̾ �غ�Ǿ��� ��� ���� ������ ��ȯ
            if (readyPlayerCount == 2)
            {

                Debug.Log("2�� �غ� �Ϸ�! ���� ������ �̵��մϴ�.");
                PhotonNetwork.LoadLevel(Scene); // ��ȯ�� �� �̸����� ����

            }
            else
            {
                return;
            }
        }
        Debug.Log("��ư�� ���Ƚ��ϴ�");
    }
    //JHScenes2 , JHScenes3 , MainScenes 
    [PunRPC]
    public void PlayerReady()
    {
        readyPlayerCount++; //���� ī��Ʈ 1 ����
    }
}
