using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon ���� ���̺귯�� �߰�

public class InElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // �Ҵ�� ���������� �� 2��
    public float closeDuration = 2f; // �� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); // ���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // ���� ������ Scale

    private bool isClosing = false; // ���� ������ ������ Ȯ��
    public ElevatorTrigger elevatorTrigger;

    private bool runElevator = false;

    private void Start()
    {
        // ��������Ʈ�� �Լ� ����
        elevatorTrigger.OnPlayersTriggered += HandlePlayersTriggered;
    }

    private void HandlePlayersTriggered(Collider player1, Collider player2)
    {
        Debug.Log($"Player1: {player1.name}, Player2: {player2.name}");

        runElevator = true;
    }

    //[PunRPC]
    public void CloseDoors()
    {
        if (!runElevator && isClosing) return;
        isClosing = true;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDoorsCoroutine", RpcTarget.Others);
        }
    }


    [PunRPC]
    private void RPCDoorsCoroutine()
    {
        isClosing = true;
        StartCoroutine(CloseDoorsCoroutine());
    }


    public IEnumerator CloseDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor_SFX",gameObject);

        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   // ���� ���� 0 ~ 1 ���� �� ���
            foreach (var door in elevatorDoors)
            {
                door.localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;

            yield return null;  // ���� �����ӱ��� ���
        }

        foreach (var door in elevatorDoors)
        {
            door.localScale = closedScale;
        }

        Debug.Log("���� �������ϴ�.");
        isClosing = false; // �� �ݱ� �Ϸ�
        photonView.RPC("CheckElevatorConditions", RpcTarget.All); // ��� Ŭ���̾�Ʈ�� ���� Ȯ�� ��û
    }

    [PunRPC]
    public void CheckElevatorConditions()
    {

        if(PhotonNetwork.IsMasterClient) 
        {

            Debug.Log("���� ������ �̵��մϴ�.");
            PhotonNetwork.LoadLevel("JHScenes3");

        }
    }

}
