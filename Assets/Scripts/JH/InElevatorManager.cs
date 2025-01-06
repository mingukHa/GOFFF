using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit ���
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Photon.Pun;

public class InElevatorManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> elevatorDoors;
    [SerializeField] private GameObject inbutton1;
    [SerializeField] private GameObject inbutton2;
    public float closeDuration = 2f;
    private Vector3 closedScale = new Vector3(1, 1, 1);
    private Vector3 openScale = new Vector3(0, 1, 1);

    private bool isButton1Pressed = false;
    private bool isButton2Pressed = false;
    private bool isDoorsClosing = false; // �� ���� ���� üũ

    [SerializeField] private Image fadeImage;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // ��ư ���� �̺�Ʈ ����
    public void OnButton1SelectEnter()
    {
        if (!isButton1Pressed) // �ߺ� ȣ�� ����
        {
            photonView.RPC(nameof(RPC_OnButton1SelectEnter), RpcTarget.AllBuffered);
        }
    }

    public void OnButton2SelectEnter()
    {
        if (!isButton2Pressed) // �ߺ� ȣ�� ����
        {
            photonView.RPC(nameof(RPC_OnButton2SelectEnter), RpcTarget.AllBuffered);
        }
    }

    // ��ư 1 ���� ó��
    [PunRPC]
    private void RPC_OnButton1SelectEnter()
    {
        isButton1Pressed = true;
        CheckButtonsAndCloseDoors();
    }

    // ��ư 2 ���� ó��
    [PunRPC]
    private void RPC_OnButton2SelectEnter()
    {
        isButton2Pressed = true;
        CheckButtonsAndCloseDoors();
    }

    // �� ��ư ��� ������ �� �� �ݰ�, ���̵� �ƿ� ����
    private void CheckButtonsAndCloseDoors()
    {
        if (isButton1Pressed && isButton2Pressed && !isDoorsClosing)
        {
            isDoorsClosing = true; // �ߺ� ȣ�� ����
            photonView.RPC(nameof(CloseDoors), RpcTarget.AllBuffered);
        }
    }

    // �� �ݴ� �Լ�
    [PunRPC]
    public void CloseDoors()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    private IEnumerator CloseDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;
            foreach (var door in elevatorDoors)
            {
                door.localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var door in elevatorDoors)
        {
            door.localScale = closedScale;
        }

        photonView.RPC(nameof(StartFadeOutAndLoadScene), RpcTarget.AllBuffered, "JHScenes2");
    }

    // ���̵� �ƿ� �� ��� �ε�
    [PunRPC]
    private void StartFadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        PhotonNetwork.LoadLevel("JHScenes2");
    }
}
