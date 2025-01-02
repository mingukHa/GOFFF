using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //엘리베이터 4개
    public float openDuration = 2f; //문 열리는 시간
    private Vector3 closedScale = new Vector3(0,0,0); //닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(1,0,0);   //열린 상태의 Scale

    private void Start()
    {
        
    }

    private void OpenDoors()
    {
        StartCoroutine(OpenDoorsCoroutine());
    }

    private IEnumerator OpenDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(closedScale, openScale, openDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //다음 프레임까지 대기
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = openScale;
        }
    }

    public void MyFirstVRFunction()
    {
        Debug.Log("Hello, World!");
    }
}