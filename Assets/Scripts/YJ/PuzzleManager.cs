using Photon.Pun;
using UnityEngine;

public class PuzzleManager : MonoBehaviourPunCallbacks
{
    public SocketValidator[] socketValidators; // ���ð��� �濡 �ִ� ��� SocketValidator
    public BoxCollider eVButton; // ���������� ��ư
    [SerializeField]
    private GameObject Monster; // ���� Prefab (Resources ������ �־�� ��)
    [SerializeField]
    private GameObject zom;
    [SerializeField]
    private GameObject zom1;

    private void Start()
    {
        eVButton.enabled = false;
    }

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
        if (eVButton != null)
        {
            eVButton.enabled = true;
            Debug.Log("���������� ��ư Ȱ��ȭ!");

            // ���� ���� ȣ��
            Monster.SetActive(true);
            SoundManager.instance.SFXPlay("ZomShout_SFX", zom.gameObject);
            SoundManager.instance.SFXPlay("ZomShout_SFX", zom1.gameObject);

            //MonsterSpawn();
            //photonView.RPC("MonsterSpawn", RpcTarget.All);
        }
        else
        {
            Debug.LogError("���������� ��ư�� ã�� �� �����ϴ�!");
        }
    }

    //[PunRPC]
    //private void MonsterSpawn()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        Monster.SetActive(true);
    //    }
    //}
}
