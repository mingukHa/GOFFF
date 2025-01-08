using Photon.Pun;
using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // �� ��ư�� ��Ÿ���� ����
    public Transform screenDisplayPoint; // ���ڰ� ǥ�õ� ȭ���� �θ� Transform
    public GameObject numberPrefab; // ǥ���� ���� ������

    private KeypadManager keypadManager;

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
    }

    public void OnPressed()
    {
        Debug.Log(number + "�� ��ư�� �������ϴ�.");

        // RPC�� ���� ���� �߰� ������ ����ȭ
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("AddNumber", RpcTarget.All, number);
    }
}
