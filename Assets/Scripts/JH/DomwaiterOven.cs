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
            //PhotonView�� ���� ��� �÷��̾�� RPC ȣ��
            photonView.RPC("MoveItemToTarget", RpcTarget.All,
                obj.GetPhotonView().ViewID, targetPosition.position);
        }

        objectsInZone.Clear();  //�ڽ�[0]���� ������ ����
    }

    [PunRPC]
    private void MoveItemToTarget(int viewID, Vector3 targetPosition)
    {
        PhotonView itemView = PhotonView.Find(viewID);
        if (itemView != null)
        {
            itemView.transform.position = targetPosition;
        }
    }
}
