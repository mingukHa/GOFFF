using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //���������� 4��
    public float openDuration = 2f; //�� ������ �ð�
    private Vector3 closedScale = new Vector3(1, 1, 1); //���� ������ Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //���� ������ Scale

    public bool isDownDoorOpening = false; //���� ������ ������ Ȯ��

    [PunRPC]
    public void CmdOpenDoors()
    {
        if (isDownDoorOpening) return;

        isDownDoorOpening = true;
        StartCoroutine(OpenDoorsCoroutine());
    }

    public IEnumerator OpenDoorsCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            float t = elapsedTime / openDuration;   //���� ���� 0 ~ 1 ���� �� ���
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(closedScale, openScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //���� �����ӱ��� ���
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = openScale;
        }
    }
}