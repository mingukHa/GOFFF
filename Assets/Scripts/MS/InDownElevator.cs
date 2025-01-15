using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon ���� ���̺귯�� �߰�

public class InDownElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // �Ҵ�� ���������� �� 2��
    [SerializeField] private Transform elevatorBottom;    // ���������� �ٴ�
    public float closeDuration = 2f; // �� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); // ���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // ���� ������ Scale

    public UpElevator upElevator;   // ���� ��ư�� �������� Ȯ���ϱ� ����
    //public DownElevator downElevator;   // �Ʒ��� ��ư�� �������� Ȯ���ϱ� ����

    private bool isClosing = false; // ���� ������ ������ Ȯ��
    private int isButtonOn = 0; // ��ư ���¸� ���ÿ��� ����
    public ElevatorDownTrigger elevatorTrigger;
    private bool runElevator = false;
    private Transform playerTr;
    private Transform ThreeFloorTp;


    //private void OnTriggerEnter(Collider other)
    //{
    //    // Collider�� ������ ������Ʈ�� �÷��̾����� Ȯ��
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        GameObject player = other.gameObject;

    //        if (player == PhotonNetwork.LocalPlayer.TagObject as GameObject)
    //        {
    //            Debug.Log("���� �÷��̾ ���������Ϳ� ���Խ��ϴ�.");
    //            photonView.RPC("CheckElevatorConditions", RpcTarget.All);
    //        }
    //    }
    //}


    private void Start()
    {
        // ��������Ʈ�� �Լ� ����
        elevatorTrigger.OnPlayerTriggered += HandlePlayersTriggered;
    }

    private void HandlePlayersTriggered(Collider player)
    {
        Debug.Log("{player.name}�� ���������Ϳ� ���Խ��ϴ�.");

        runElevator = true;
        playerTr = player.transform;
    }

    // CloseDoors�� �̺�Ʈ�� ����س�����
    public void CloseDoors()
    {
        //isButtonOn++;
        //photonView.RPC("RPCIsButtonOn", RpcTarget.Others);
        if (!runElevator) return;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDownDoors", RpcTarget.Others);
        }
    }
    //[PunRPC]
    //public void RPCIsButtonOn()
    //{
    //    isButtonOn++;
    //}

    [PunRPC]
    private void RPCDownDoors()
    {
        isClosing = true;
        StartCoroutine(CloseDoorsCoroutine());
    }


    public IEnumerator CloseDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor2_SFX");

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
        photonView.RPC("DownElevatorTp", RpcTarget.All); // ��� Ŭ���̾�Ʈ�� ���� Ȯ�� ��û
    }

    [PunRPC]
    private void DownElevatorTp()
    {
        playerTr.position = ThreeFloorTp.position;
    }
}
