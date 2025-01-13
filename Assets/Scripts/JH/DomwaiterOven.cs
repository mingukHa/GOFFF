using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DomwaiterOven : MonoBehaviourPun
{
    public List<GameObject> objectsInZone = new List<GameObject>(); //�浹�� ������Ʈ�� ���� ����Ʈ

    private void OnTriggerEnter(Collider other)
    {
        // Trigger Zone�� ���� ������Ʈ�� ����Ʈ�� �߰�
        if (!objectsInZone.Contains(other.gameObject))
        {
            objectsInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Trigger Zone���� ���� ������Ʈ�� ����Ʈ���� ����
        if (objectsInZone.Contains(other.gameObject))
        {
            objectsInZone.Remove(other.gameObject);
        }
    }

    public List<GameObject> GetObjectsInZone()
    {
        // ���� Trigger Zone�� �ִ� ������Ʈ ����Ʈ ��ȯ
        return objectsInZone;
    }

    // ������ ���� �޼���
    public void SendItemsToTarget(Transform targetPosition)
    {
        foreach (GameObject obj in objectsInZone)
        {
            // �����ϴ� �������� ȸ��, ��ġ�� ����
            obj.transform.SetPositionAndRotation
                (targetPosition.position, targetPosition.rotation);
        }
        objectsInZone.Clear();//�ڽ�[0]���� ������ ����
    }
}