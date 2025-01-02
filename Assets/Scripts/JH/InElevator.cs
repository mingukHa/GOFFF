using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class InElevator : MonoBehaviour
{
    [SerializeField] private Elevator elevator;  //Elevator 스크립트를 가지고있는 elevator변수
    private bool isPushed = false;  //버튼 누른지 체크

    [SerializeField] private List<Transform> elevatorDoors; //엘리베이터 문들
    public float closeDuration = 2f; // 문 닫히는 시간
    private Vector3 closedScale = new Vector3(0, 0, 0); // 닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(1, 0, 0);   // 열린 상태의 Scale
    private bool isClosing = false;

    [SerializeField] private InElevatorManager elevatorManager;

    private void CloseDoors()
    {
        if(!isClosing)
        {
            StartCoroutine(CloseDoorsCoroutine());
            elevatorManager.CloseDoorsDetected();
        }
    }

    private IEnumerator CloseDoorsCoroutine()
    {
        isClosing = true;
        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = 
                    Vector3.Lerp(openScale, closedScale, elapsedTime / closeDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }

        SceneManager.LoadScene("JHScenes2");
        isClosing = false;
    }
}