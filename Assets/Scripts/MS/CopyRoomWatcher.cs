using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CopyRoomWatcher : MonoBehaviour
{
    public Transform[] objects; // �˻��� ������Ʈ ����Ʈ
    public Transform[] targetsTr;         // ��ǥ ��ġ
    public float checkRadius = 4.0f;       // ��ǥ ��ġ ��� ����(�ݰ�)
    private int objectsAtTarget = 0;       // ��ǥ ��ġ�� ������ ������Ʈ ����
    private bool elevatorOpen = false;

    public BoxCollider buttoncollider = null;

    IEnumerator watch = null;

    public bool ElevatorOpen {  get { return elevatorOpen; } }

    private void Start()
    {
        watch = WatchCoroutine();
        StartCoroutine(watch);
        buttoncollider.enabled = false;
    }

    private IEnumerator WatchCoroutine()
    {
        objectsAtTarget = 0; // �� �����Ӹ��� �ʱ�ȭ

        for(int i = 0;  i < objects.Length; i++)
        {
            if (objects[i] == null) continue; // ������Ʈ�� ������ ����

            // ������Ʈ�� ��ǥ ��ġ �� �Ÿ� ���
            float distance = Vector3.Distance(objects[i].position, targetsTr[i].position);

            if (distance <= checkRadius)
            {
                objectsAtTarget++; // ��ǥ ��ġ�� ������ ������Ʈ ���� ����
            }
        }

        // ���� ���� Ȯ��
        if (objectsAtTarget >= 5)
        {
            Debug.Log("5���� ������Ʈ�� ��ǥ ��ġ�� �����߽��ϴ�!");
            // ���� ���� �� �߰� ���� ����
            StopCoroutine(watch);
            buttoncollider.enabled = true;
        }

        yield return new WaitForSeconds(1f);
    }
}
