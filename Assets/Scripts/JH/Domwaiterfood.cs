using UnityEngine;
using Photon.Pun;

public class Domwaiterfood : MonoBehaviourPun
{
    [SerializeField] private DomwaiterOven triggerZone0;   // ��Ȳ�� �ڽ� [0]
    [SerializeField] private Transform targetPosition;  // ��Ȳ�� �ڽ� [1]
  

    

    public void OnButtonPressed()
    {
        
        /*  ��Ƽ�� �ƴ� ��Ȳ���� ���� ����
        
        // ��Ȳ�� �ڽ�[0]dptj �浹�� ��� ������Ʈ ��������
        var objectsToMove = triggerZone0.GetObjectsInZone();

        // ��� ������Ʈ�� ��Ȳ�� �ڽ�[1] ��ġ�� �̵�
        foreach (GameObject obj in objectsToMove)
        {
            obj.transform.position = targetPosition.position;
        }

        */

        //if (photonView.IsMine) // �� ��ư�� ���� �÷��̾ ������ ���� ��û�� ����
        //{
        //    // ��Ȳ�� �ڽ�[0]���� �������� ����
            triggerZone0.SendItemsToTarget(targetPosition);
        //}
    }
}