using UnityEngine;
using System.Collections.Generic;

public class InElevatorManager : MonoBehaviour
{
    private List<InElevator> elevatorObjects = new List<InElevator>(); // 모든 InElevator 오브젝트 리스트

    private void Start()
    {
        // 씬에 있는 모든 InElevator 오브젝트를 찾기
        elevatorObjects.AddRange(FindObjectsOfType<InElevator>());
    }

    public void CloseDoorsDetected()
    {
        // 모든 InElevator에서 CloseDoors()가 호출될 때마다 반응
        Debug.Log("어떤 InElevator에서 CloseDoors()가 호출되었습니다.");
        // 여기에 추가적인 처리를 할 수 있음
    }

    // InElevator 스크립트에서 호출할 메서드
    public void RegisterCloseDoors(InElevator inElevator)
    {
        if (!elevatorObjects.Contains(inElevator))
        {
            elevatorObjects.Add(inElevator);
        }
    }

    public void UnregisterCloseDoors(InElevator inElevator)
    {
        if (elevatorObjects.Contains(inElevator))
        {
            elevatorObjects.Remove(inElevator);
        }
    }
}
