using Photon.Pun;
using UnityEngine;

public class f1layerspawn : MonoBehaviour
{
    public Transform[] TagObject; // �迭�� PlayerPos1, PlayerPos2 ��ġ ����

    private void Awake()
    {
        // PlayerPos1, PlayerPos2 ��ġ ã��
        GameObject playerPos1 = GameObject.Find("PlayerHolder(Clone)");
        GameObject playerPos2 = GameObject.Find("PlayerHolder1(Clone)");

        if (playerPos1 == null)
        {
            Debug.LogError("PlayerPos1�� ���� �����ϴ�!");
            return;
        }
        if (playerPos2 == null)
        {
            Debug.LogError("PlayerPos2�� ���� �����ϴ�!");
            return;
        }

        // ���� �÷��̾��� ActorNumber�� �������� ��ġ ����
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

        if (playerIndex < 1 || playerIndex > TagObject.Length)
        {
            Debug.LogError($"�߸��� �÷��̾� �ε���: {playerIndex}. TagObject �迭�� ũ�⸦ Ȯ���ϼ���.");
            return;
        }

        if (playerIndex == 1)
        {
            // ù ��° �÷��̾�� PlayerPos1 ��ġ�� �̵�
            playerPos1.transform.position = TagObject[playerIndex - 1].position; // �ε��� ����
            Debug.Log("�÷��̾� 1�� PlayerPos1���� �̵��Ǿ����ϴ�.");
        }
        else if (playerIndex == 2)
        {
            // �� ��° �÷��̾�� PlayerPos2 ��ġ�� �̵�
            playerPos2.transform.position = TagObject[playerIndex - 1].position; // �ε��� ����
            Debug.Log("�÷��̾� 2�� PlayerPos2�� �̵��Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("�������� �ʴ� �÷��̾� �ε����Դϴ�.");
        }
    }
}
