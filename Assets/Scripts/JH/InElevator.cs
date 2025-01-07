using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun; // Photon ���� ���̺귯�� �߰�

public class InElevator : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> elevatorDoors; //�Ҵ�� ���������� �� 2��
    [SerializeField] private Transform elevatorBottom;    //���������� �ٴ�
    public float closeDuration = 2f; //�� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); //���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //���� ������ Scale

    public UpElevator upElevator;   //���� ��ư�� �������� Ȯ���ϱ� ����
    public DownElevator downElevator;   //�Ʒ��� ��ư�� �������� Ȯ���ϱ� ����

    private bool isPlayerOnElevator = false; // �÷��̾ ���������� �ٴڿ� �ִ��� Ȯ��

    [PunRPC]
    public void CloseDoors()
    {
        photonView.RPC("CloseDoorsRPC", Photon.Pun.RpcTarget.All); // ��� Ŭ���̾�Ʈ���� ���� �ݵ��� RPC ȣ��
    }

    [PunRPC]
    private void CloseDoorsRPC()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    public IEnumerator CloseDoorsCoroutine()
    {
        // �� �� �̻��� �÷��̾ ���� ��� �� �ݱ�, �ڷ���Ʈ, �� �̵� ����
        if (IsMultiplePlayersOnElevator())
        {
            Debug.Log("�� �� �̻��� �÷��̾ ž�� ���Դϴ�. ����� ���ѵ˴ϴ�.");
            yield break; // �Լ� ����, �� �ݱ� �� ���� �ڵ� ���� �� ��
        }

        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   //���� ���� 0 ~ 1 ���� �� ���
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //���� �����ӱ��� ���
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }

        if (isPlayerOnElevator)
        {
            CheckElevatorConditions();
        }
    }

    private void CheckElevatorConditions()
    {
        // �� �� �̻��� �÷��̾ ���� ��� �� �̵�, �ڷ���Ʈ ����
        if (IsMultiplePlayersOnElevator())
        {
            Debug.Log("�� �� �̻��� �÷��̾ ž�� ���Դϴ�. ����� ���ѵ˴ϴ�.");
            return; // �� �̵� �� �ڷ���Ʈ �������� ����
        }

        // UpElevator�� isUpDoorOpening�� true�� �� ���� ������ �̵�
        if (upElevator != null && upElevator.isUpDoorOpening)
        {
            LoadNextScene();
        }
        // DownElevator�� isDownDoorOpening�� true�� �� �ڷ���Ʈ
        else if (downElevator != null && downElevator.isDownDoorOpening)
        {
            TeleportPlayerToOrigin();
        }
        else
        {
            Debug.Log("���������� ���°� ��ȿ���� �ʽ��ϴ�.");
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ���� ���� ���� �ε��� ��������
        int nextSceneIndex = currentSceneIndex + 1;                       // ���� �� �ε��� ���

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)      // ���� ���� �ȿ� ���� �����ϴ��� Ȯ��
        {
            SceneManager.LoadScene(nextSceneIndex);                       // ���� �� �ε�
        }
        else
        {
            Debug.Log("������ ���Դϴ�. �� �̻� ���� �����ϴ�.");
        }
    }

    private void TeleportPlayerToOrigin()
    {
        GameObject player = PhotonNetwork.LocalPlayer.TagObject as GameObject; // Photon���� ���� ���� �÷��̾� ã��

        if (player != null)
        {
            player.transform.position = new Vector3(0, 0, 0); // ���� ��ǥ�� �������� (0, 0, 0) ��ġ�� �ڷ���Ʈ
            Debug.Log("�÷��̾ (0, 0, 0) ��ġ�� �ڷ���Ʈ ���׽��ϴ�.");
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    private bool IsMultiplePlayersOnElevator()
    {
        // Photon ��Ʈ��ũ���� ���������� �ٴڿ� �ִ� �÷��̾� �� üũ
        int playerCount = 0;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = player.TagObject as GameObject;
            if (playerObject != null && playerObject.GetComponent<Collider>().
                bounds.Intersects(elevatorBottom.GetComponent<Collider>().bounds))
            {
                playerCount++;
            }
        }

        return playerCount > 1; // �� �� �̻��� �÷��̾ ������ true
    }

    // ���������� �ٴڰ� �浹 ���� (���������� �÷��̾� ���� Ȯ��)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = true;
            Debug.Log("�÷��̾ ���������� �ٴڿ� ž�� ���Դϴ�.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnElevator = false;
            Debug.Log("�÷��̾ ���������� �ٴڿ��� ���Ƚ��ϴ�.");
        }
    }
}