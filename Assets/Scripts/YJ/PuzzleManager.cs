using Photon.Pun;
using UnityEngine;

public class PuzzleManager : MonoBehaviourPunCallbacks
{
    public SocketValidator[] socketValidators; // ���ð��� �濡 �ִ� ��� SocketValidator
    public GameObject jailBar; // ���� ��â�� ������Ʈ
    [SerializeField]
    private GameObject Monster; // ���� Prefab (Resources ������ �־�� ��)
    [SerializeField]
    private GameObject spawnpoint; // ���Ͱ� ������ ��ġ

    public void CheckPuzzleStatus()
    {
        if (socketValidators == null || socketValidators.Length == 0)
        {
            Debug.LogError("SocketValidators �迭�� Null�̰ų� �������� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var validator in socketValidators)
        {
            if (!validator.IsObjectCorrectlyPlaced())
            {
                Debug.Log("������ ���� Ǯ���� �ʾҽ��ϴ�.");
                return;
            }
        }
        Debug.Log("������ Ǯ�Ƚ��ϴ�!");
        photonView.RPC("SolvePuzzle", RpcTarget.All);
    }

    [PunRPC]
    private void SolvePuzzle()
    {
        if (jailBar != null)
        {
            jailBar.SetActive(false);
            Debug.Log("��â�� ��Ȱ��ȭ!");
        }
        else
        {
            Debug.LogError("��â�� ������Ʈ�� ã�� �� �����ϴ�!");
        }

        // ���� ���� ȣ��
        MonsterSpawn();
    }

    private void MonsterSpawn()
    {
        if (Monster == null || spawnpoint == null)
        {
            Debug.LogError("���� ���� ��ġ�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.Instantiate�� ���� ���� ����
            PhotonNetwork.Instantiate(Monster.name, spawnpoint.transform.position, spawnpoint.transform.rotation);
        }
    }
}
