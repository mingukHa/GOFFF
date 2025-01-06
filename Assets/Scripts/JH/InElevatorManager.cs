using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // XR Interaction Toolkit 사용
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
    private bool isDoorsClosing = false; // 문 닫힘 상태 체크

    [SerializeField] private Image fadeImage;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // 버튼 선택 이벤트 연결
    public void OnButton1SelectEnter()
    {
        if (!isButton1Pressed) // 중복 호출 방지
        {
            photonView.RPC(nameof(RPC_OnButton1SelectEnter), RpcTarget.AllBuffered);
        }
    }

    public void OnButton2SelectEnter()
    {
        if (!isButton2Pressed) // 중복 호출 방지
        {
            photonView.RPC(nameof(RPC_OnButton2SelectEnter), RpcTarget.AllBuffered);
        }
    }

    // 버튼 1 눌림 처리
    [PunRPC]
    private void RPC_OnButton1SelectEnter()
    {
        isButton1Pressed = true;
        CheckButtonsAndCloseDoors();
    }

    // 버튼 2 눌림 처리
    [PunRPC]
    private void RPC_OnButton2SelectEnter()
    {
        isButton2Pressed = true;
        CheckButtonsAndCloseDoors();
    }

    // 두 버튼 모두 눌렸을 때 문 닫고, 페이드 아웃 시작
    private void CheckButtonsAndCloseDoors()
    {
        if (isButton1Pressed && isButton2Pressed && !isDoorsClosing)
        {
            isDoorsClosing = true; // 중복 호출 방지
            photonView.RPC(nameof(CloseDoors), RpcTarget.AllBuffered);
        }
    }

    // 문 닫는 함수
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

    // 페이드 아웃 후 장면 로드
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
