using Photon.Pun;
using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // �� ��ư�� ��Ÿ���� ����
    public Transform screenDisplayPoint; // ���ڰ� ǥ�õ� ȭ���� �θ� Transform
    public GameObject numberPrefab; // ǥ���� ���� ������
    //private SoundManager soundManager; // ���� �Ŵ���
    private KeypadManager keypadManager; // Ű�е� �Ŵ���

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
        //soundManager = FindObjectOfType<SoundManager>();
    }

    public void OnPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX");
        Debug.Log(number + "�� ��ư�� �������ϴ�.");

        // RPC�� ���� ���� �߰� ������ ����ȭ
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("AddNumber", RpcTarget.Others, number);
    }
}
