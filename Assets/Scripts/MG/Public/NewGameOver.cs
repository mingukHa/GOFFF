using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
public class NewGameOver : MonoBehaviourPun
{
    private GameObject Player1; // ���� �÷��̾�
    private GameObject Player2; // ��� �÷��̾�
    private GameObject monster;

    [SerializeField] private Transform spwan1;       // �÷��̾�1 ���� ����
    [SerializeField] private Transform spwan2;       // �÷��̾�2 ���� ����
    [SerializeField] private Transform monsterspawn; // ���� ���� ����

    private void Start()
    {
        StartCoroutine(WaitForPlayersAndMonster()); // �÷��̾�� ���� ���� Ȯ��
    }

    private IEnumerator WaitForPlayersAndMonster()
    {
        while (Player1 == null || Player2 == null || monster == null)
        {
            Debug.Log("�÷��̾� �� ���� ���� ��õ� ��...");
            FindPlayersAndMonster();
            yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� ��õ�
        }
        Debug.Log("�÷��̾� �� ���� ���� �Ϸ�.");
    }

    private void FindPlayersAndMonster()
    {
        foreach (PhotonView view in PhotonNetwork.PhotonViews)
        {
            if (view != null && view.Owner != null)
            {
                if (view.Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    Player1 = view.gameObject; // ���� �÷��̾�
                    Debug.Log("Player1 �Ҵ� �Ϸ�.");
                }
                else if (view.Owner.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    Player2 = view.gameObject; // ��� �÷��̾�
                    Debug.Log("Player2 �Ҵ� �Ϸ�.");
                }
            }
        }

        // ���ʹ� �±׷� ã��
        monster = GameObject.FindWithTag("Monster");
        if (monster != null)
        {
            Debug.Log("Monster �Ҵ� �Ϸ�.");
        }
        else
        {
            Debug.LogError("Monster�� ã�� �� �����ϴ�.");
        }
    }

    [PunRPC]
    public void ReStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ReStart ȣ��: ��ġ�� ���� ���� �������� ���� ��...");
            photonView.RPC("UpdatePlayerPosition", RpcTarget.All, spwan1.position, spwan2.position, monsterspawn.position);
        }
    }

    [PunRPC]
    private void UpdatePlayerPosition(Vector3 position1, Vector3 position2, Vector3 monsterPosition)
    {
        // Player1 ��ġ ����
        if (Player1 != null)
        {
            UpdatePosition(Player1, position1);
            Debug.Log($"Player1 ��ġ ���� �Ϸ�: {position1}");
        }

        // Player2 ��ġ ����
        if (Player2 != null)
        {
            UpdatePosition(Player2, position2);
            Debug.Log($"Player2 ��ġ ���� �Ϸ�: {position2}");
        }

        // Monster ��ġ ����
        if (monster != null)
        {
            UpdatePosition(monster, monsterPosition, true); // ���ʹ� NavMeshAgent ó��
            Debug.Log($"Monster ��ġ ���� �Ϸ�: {monsterPosition}");
        }
    }

    private void UpdatePosition(GameObject obj, Vector3 newPosition, bool isMonster = false)
    {
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // ���� ��� ��Ȱ��ȭ
            obj.transform.position = newPosition; // ��ġ ����
            rb.isKinematic = false; // ���� ��� �ٽ� Ȱ��ȭ
        }
        else
        {
            obj.transform.position = newPosition; // Rigidbody�� ���� ���
        }

        // ������ ��� NavMeshAgent ó��
        if (isMonster)
        {
            var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(newPosition);
            }
        }
    }
}
