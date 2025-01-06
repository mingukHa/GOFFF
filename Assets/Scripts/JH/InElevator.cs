using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //���������� 4��
    public float closeDuration = 2f; //�� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); //���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //���� ������ Scale

    public void CloseDoors()
    {
        StartCoroutine(CloseDoorsCoroutine());
    }

    public IEnumerator CloseDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < closeDuration)
        {
            float t = elapsedTime / closeDuration;   //���� ���� 0 ~ 1 ���� �� ���
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(openScale, closedScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //���� �����ӱ��� ���
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = closedScale;
        }
    }
}