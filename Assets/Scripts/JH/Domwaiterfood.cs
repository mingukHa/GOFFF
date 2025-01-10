using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    [SerializeField] private DomwaiterOven triggerZone0;   // ��Ȳ�� �ڽ� [0]
    [SerializeField] private Transform targetPosition;  // ��Ȳ�� �ڽ� [1]

    public void OnButtonPressed()
    {
        // ��Ȳ�� �ڽ�[0]dptj �浹�� ��� ������Ʈ ��������
        var objectsToMove = triggerZone0.GetObjectsInZone();

        // ��� ������Ʈ�� ��Ȳ�� �ڽ�[1] ��ġ�� �̵�
        foreach (GameObject obj in objectsToMove)
        {
            obj.transform.position = targetPosition.position;
        }
    }
}