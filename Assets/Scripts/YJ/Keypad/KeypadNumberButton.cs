using Photon.Pun;
using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // �� ��ư�� ��Ÿ���� ����
    //public Transform screenDisplayPoint; // ���ڰ� ǥ�õ� ȭ���� �θ� Transform
    //public GameObject numberPrefab; // ǥ���� ���� ������
    private KeypadManager keypadManager; // Ű�е� �Ŵ���

    private void Start()
    {
        keypadManager = FindFirstObjectByType<KeypadManager>();
    }

    public void OnPressed()
    {
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);
        //Debug.Log(number + "�� ��ư�� �������ϴ�.");

        // RPC�� ���� ���� �߰� ������ ����ȭ
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("AddNumber", RpcTarget.Others, number);
    }
}
