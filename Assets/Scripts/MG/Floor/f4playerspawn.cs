using Photon.Pun;
using UnityEngine;

public class f4playerspawn : MonoBehaviour
{
    public Transform[] TagObject;
    private void Awake()
    {
        // PlayerPos1, PlayerPos2 ��ġ ã��
        GameObject playerPos1 = GameObject.Find("PlayerHolder(Clone)");
        GameObject playerPos2 = GameObject.Find("PlayerHolder(Clone)");
        if (playerPos1 == null)
        {
            Debug.LogError($"{playerPos1} ���� �����ϴ�!");
            return;
        }
        if (playerPos2 == null)
        {
            Debug.LogError($"{playerPos2} ���� �����ϴ�!");
            return;
        }
        // ���� �÷��̾��� ActorNumber�� �������� ��ġ ����
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        if (playerIndex == 1)
        {
            // ù ��° �÷��̾�� PlayerPos1 ��ġ�� �̵�
            TagObject[playerIndex].position = playerPos1.transform.position;
            Debug.Log("�÷��̾� 1�� PlayerPos1���� �̵��Ǿ����ϴ�.");
        }
        else if (playerIndex == 2)
        {
            // �� ��° �÷��̾�� PlayerPos2 ��ġ�� �̵�
            TagObject[playerIndex].position = playerPos2.transform.position;
            Debug.Log("�÷��̾� 2�� PlayerPos2�� �̵��Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("�������� �ʴ� �÷��̾� �ε����Դϴ�.");
        }
    }
}
