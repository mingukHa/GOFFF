using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun; // Photon ���� ���̺귯�� �߰�

public class InElevator : MonoBehaviourPun
{
    [SerializeField] private List<Transform> elevatorDoors; // �Ҵ�� ���������� �� 2��
    //[SerializeField] private Transform elevatorBottom;    // ���������� �ٴ�
    public float closeDuration = 2f; // �� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); // ���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   // ���� ������ Scale

    //public UpElevator upElevator;   // ���� ��ư�� �������� Ȯ���ϱ� ����
    //public DownElevator downElevator;   // �Ʒ��� ��ư�� �������� Ȯ���ϱ� ����

    private bool isClosing = false; // ���� ������ ������ Ȯ��
    private int isButtonOn = 0; // ��ư ���¸� ���ÿ��� ����
    public ElevatorTrigger elevatorTrigger;

    private bool runElevator = false;


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
        //isButtonOn++;
        //photonView.RPC("RPCIsButtonOn", RpcTarget.Others);
        if (!runElevator && isClosing) return;
        isClosing = true;

        if (photonView.IsMine)
        {
            StartCoroutine(CloseDoorsCoroutine());
            photonView.RPC("RPCDoorsCoroutine", RpcTarget.Others);
        }
    }
    //[PunRPC]
    //public void RPCIsButtonOn()
    //{
    //    isButtonOn++;
    //}

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
        // UpElevator�� isUpDoorOpening�� true�� �� ���� ������ �̵�
        //if (upElevator != null && upElevator.isUpDoorOpening)
        if(PhotonNetwork.IsMasterClient) 
        {
            //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //int nextSceneIndex = currentSceneIndex + 1;
            //Debug.Log($"{currentSceneIndex}�ε��� ��{nextSceneIndex}������");

            //if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            //{
                Debug.Log("���� ������ �̵��մϴ�.");
                PhotonNetwork.LoadLevel("JHScenes3");
            //}
            //else
            //{
            //    Debug.Log("������ ���Դϴ�. �� �̻� ���� �����ϴ�.");
            //}
        }
        //else
        //{
        //    Debug.Log("���������� ���°� ��ȿ���� �ʽ��ϴ�.");
        //}
    }

    //[PunRPC]
    //public void LoadNextScene()
    //{
    //    //if (isButtonOn >= 2) return; // ���� �̹� �ε�Ǿ����� Ȯ��

    //    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //    int nextSceneIndex = currentSceneIndex + 1;
    //    Debug.Log($"{currentSceneIndex}�ε��� ��{nextSceneIndex}������");
        
    //    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
    //    {
    //        Debug.Log("���� ������ �̵��մϴ�.");           
    //        PhotonNetwork.LoadLevel(nextSceneIndex);
    //    }
    //    else
    //    {
    //        Debug.Log("������ ���Դϴ�. �� �̻� ���� �����ϴ�.");
    //    }
    //}
}
