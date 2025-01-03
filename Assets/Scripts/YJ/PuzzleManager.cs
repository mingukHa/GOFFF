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
            Debug.LogError("SocketValidators array is empty or not assigned!");
            return;
        }

        foreach (var validator in socketValidators)
        {
            if (!validator.IsObjectCorrectlyPlaced())
            {
                Debug.Log("Puzzle not solved yet.");
                return;
            }
        }
        Debug.Log("Puzzle solved!");
        SolvePuzzle();
    }

    private void SolvePuzzle()
    {
        if (jailBar != null)
        {
            jailBar.SetActive(false);
            Debug.Log("Jailbar disabled!");
        }
        else
        {
            Debug.LogError("Jailbar not found!");
        }

        // ���� ���� ȣ��
        MonsterSpawn();
    }

    private void MonsterSpawn()
    {
        if (Monster == null || spawnpoint == null)
        {
            Debug.LogError("Monster or spawnpoint is not assigned!");
            return;
        }

        // PhotonNetwork.Instantiate�� ���� ���� ����
        PhotonNetwork.Instantiate(Monster.name, spawnpoint.transform.position, spawnpoint.transform.rotation);
    }
}
