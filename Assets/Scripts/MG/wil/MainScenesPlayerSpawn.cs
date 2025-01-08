using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
public class MainScenesPlayerSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefab; //������ �迭
    [SerializeField] private Transform[] spawnPoints; //���� ��ġ
    private bool hasSpawned = false; //���� Ȯ�ο�
    private void Start()
    {
        if (!PhotonNetwork.IsConnected) //���� ���� ����
        {
            Debug.Log("���� ������ ������ �� �Ǿ��� �κ�� �̵�");
            SceneManager.LoadScene("LoginScenes");
            return;
        }
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("���� �濡 �������� �ʾҽ��ϴ�. �� ������ ��ٸ��ϴ�...");
            return;
        }
        StartCoroutine(WaitForRoomReady()); //�� ���� �� �÷��̾� ����
    }
    public override void OnJoinedRoom() //�濡 ���� �ϸ� ȣ��
    {
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}");

        if (!hasSpawned)
        {
            SpawnPlayer();//������ �� �Ǿ� ������ ����
        }
    }
    private IEnumerator WaitForRoomReady()
    {//��Ʈ��ũ ���� �� �� ���� ���
        yield return new WaitUntil(() => PhotonNetwork.IsConnected && PhotonNetwork.InRoom);
        SpawnPlayer();//������ �Ϸ� �Ǹ� �������� ����
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null) //�������� ��
        {
            Debug.LogError("Player Prefab�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0) //���� ����Ʈ�� ��
        {
            Debug.LogError("���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //�迭��ȣ �ް�
        Transform spawnPoint = spawnPoints[playerIndex % spawnPoints.Length]; //�迭 ��ȣ�� �´� ��ġ�� ����        

        GameObject player = PhotonNetwork.Instantiate(playerPrefab[playerIndex].name, spawnPoint.position, spawnPoint.rotation);
        //�÷��̾ ���� ��Ʈ��ũ�� ���� �� �迭 ��ȣ�� �´� ��ġ�� ������,��ġ ����
        if (player != null)
        {
            Debug.Log($"�÷��̾� {PhotonNetwork.LocalPlayer.NickName}��(��) ��ġ {spawnPoint.position}�� �����Ǿ����ϴ�.");
            hasSpawned = true;
        }//������ �Ǿ��ٸ� Ʈ��� �����ؼ� �ߺ� ���� ����
        else
        {
            Debug.LogError("�÷��̾� ������ ������ �����߽��ϴ�!");
        }
       
    }     
    
}
