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

    [SerializeField] private Image fadeImage;

    // 버튼 선택이벤트 연결
    [PunRPC]
    public void OnButton1SelectEnter()
    {
        isButton1Pressed = true;
        CheckButtonsAndCloseDoors();
    }
    [PunRPC]
    public void OnButton2SelectEnter()
    {
        isButton2Pressed = true;
        CheckButtonsAndCloseDoors();
    }

    // 두 버튼 모두 눌렸을 때 문 닫고, 페이드 아웃 시작
    [PunRPC]
    private void CheckButtonsAndCloseDoors()
    {
        if (isButton1Pressed && isButton2Pressed)
        {
            CloseDoors();
            StartCoroutine(FadeOutAndLoadScene("JHScenes2"));
        }
    }

    // 문 닫는 함수
    [PunRPC]
    public void CloseDoors()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }
    [PunRPC]
    private IEnumerator CloseDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }
    }

    // 페이드 아웃 후 장면 로드
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
