using Photon.Pun;
using UnityEngine;
using System.Collections;

public class f1playerspawn : MonoBehaviour
{
    public Transform[] TagObject; // �迭�� PlayerPos1, PlayerPos2 ��ġ ����
    private GameObject handOffset;
    private void Awake()
    {
        // PlayerPos1, PlayerPos2 ��ġ ã��
        GameObject playerPos1 = GameObject.Find("PlayerHolder(Clone)");
        GameObject playerPos2 = GameObject.Find("PlayerHolder1(Clone)");
        handOffset = GameObject.Find("handOffset") ?? transform.Find("handOffset")?.gameObject;


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
    private void Start()
    {
        StartCoroutine(FindAndToggleHandOffset());
    }
    private IEnumerator FindAndToggleHandOffset()
    {
        // handOffset GameObject�� ã�� ��� (�ִ� 1�� ���)
        float timeout = 1f;
        float elapsedTime = 0f;

        while (handOffset == null && elapsedTime < timeout)
        {
            handOffset = GameObject.Find("handOffset");
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        if (handOffset == null)
        {
            Debug.LogError("handOffset GameObject�� ã�� �� �����ϴ�.");
            yield break; // �ڷ�ƾ ����
        }

        Debug.Log("handOffset GameObject�� ã�ҽ��ϴ�.");

        // handOffset GameObject ��Ȱ��ȭ
        handOffset.SetActive(false);
        Debug.Log("handOffset ��Ȱ��ȭ��.");

        // 0.5�� ���
        yield return new WaitForSeconds(0.5f);

        // handOffset GameObject Ȱ��ȭ
        handOffset.SetActive(true);
        Debug.Log("handOffset Ȱ��ȭ��.");
    }
}

