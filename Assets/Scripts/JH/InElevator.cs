using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class InElevator : MonoBehaviour
{
    [SerializeField] private Elevator elevator;  //Elevator ��ũ��Ʈ�� �������ִ� elevator����
    private bool isPushed = false;  //��ư ������ üũ

    [SerializeField] private List<Transform> elevatorDoors; //���������� ����
    public float closeDuration = 2f; // �� ������ �ð�
    private Vector3 closedScale = new Vector3(0, 0, 0); // ���� ������ Scale
    private Vector3 openScale = new Vector3(1, 0, 0);   // ���� ������ Scale
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
            yield return null; // ���� �����ӱ��� ���
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }

        SceneManager.LoadScene("JHScenes2");
        isClosing = false;
    }
}