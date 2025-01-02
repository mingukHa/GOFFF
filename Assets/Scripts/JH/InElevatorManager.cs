using UnityEngine;
using System.Collections.Generic;

public class InElevatorManager : MonoBehaviour
{
    private List<InElevator> elevatorObjects = new List<InElevator>(); // ��� InElevator ������Ʈ ����Ʈ

    private void Start()
    {
        // ���� �ִ� ��� InElevator ������Ʈ�� ã��
        elevatorObjects.AddRange(FindObjectsOfType<InElevator>());
    }

    public void CloseDoorsDetected()
    {
        // ��� InElevator���� CloseDoors()�� ȣ��� ������ ����
        Debug.Log("� InElevator���� CloseDoors()�� ȣ��Ǿ����ϴ�.");
        // ���⿡ �߰����� ó���� �� �� ����
    }

    // InElevator ��ũ��Ʈ���� ȣ���� �޼���
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
