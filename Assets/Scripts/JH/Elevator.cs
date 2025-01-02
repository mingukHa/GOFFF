using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //���������� 4��
    public float openDuration = 2f; //�� ������ �ð�
    private Vector3 closedScale = new Vector3(0,0,0); //���� ������ Scale
    private Vector3 openScale = new Vector3(1,0,0);   //���� ������ Scale

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
            yield return null;  //���� �����ӱ��� ���
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