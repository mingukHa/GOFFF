using UnityEngine;
using System.Collections;
using Photon.Pun;
public class F1Button : MonoBehaviourPun //1�� ��ư ����
{
    public float MoveSpeed = 5f; // �̵� �ӵ�
    public GameObject door;
    public GameObject target;

    private Coroutine moveCoroutine; // �̵� Coroutine ����
    [PunRPC]
    public void ButtonMove()
    {
        // �̹� ���� ���� Coroutine�� �ִٸ� ����
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        // ���ο� Coroutine ����
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);
        SoundManager.instance.SFXPlay("Shutter_SFX", this.gameObject);
        moveCoroutine = StartCoroutine(MoveDoor());
        photonView.RPC("ButtonMove", RpcTarget.Others);
    }
    
    private IEnumerator MoveDoor()
    {
        while (Vector3.Distance(door.transform.position, target.transform.position) > 0.01f)
        {
            // �ε巴�� ��ǥ ��ġ�� �̵�
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                target.transform.position,
                MoveSpeed * Time.deltaTime
            );

            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� �Ϸ� �� ��Ȯ�� ��ǥ ��ġ�� ����
        door.transform.position = target.transform.position;

        // Coroutine ���� �ʱ�ȭ
        moveCoroutine = null;
    }
}
