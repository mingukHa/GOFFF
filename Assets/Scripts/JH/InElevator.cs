using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //엘리베이터 4개
    public float closeDuration = 2f; //문 닫히는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); //닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //열린 상태의 Scale

    public void CloseDoors()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    public IEnumerator CloseDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   //보간 비율 0 ~ 1 사이 값 계산
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //다음 프레임까지 대기
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }
    }
}