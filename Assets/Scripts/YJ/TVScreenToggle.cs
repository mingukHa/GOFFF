using UnityEngine;
using Photon.Pun;

public class TVScreenToggle : MonoBehaviourPun
{
    public GameObject screen1; // Screen 1
    public GameObject screen2; // Screen 2
    private bool isScreen1Active = true; // Screen 1�� �ʱ� Ȱ�� ����

    private void Start()
    {
        // �ʱ� ���� ����
        UpdateScreen(isScreen1Active);
    }

    public void ToggleScreen()
    {
        if (PhotonNetwork.IsConnected)
        {
            // RPC ȣ��� ��� �÷��̾�� TV ���� ����ȭ
            photonView.RPC("ToggleScreenRPC", RpcTarget.AllBuffered);
        }
        else
        {
            // ��Ʈ��ũ�� �ƴ� ���ÿ����� ����
            UpdateScreen(!isScreen1Active);
        }
    }

    [PunRPC]
    private void ToggleScreenRPC()
    {
        // Ȱ�� ���� ��ȯ �� ȭ�� ������Ʈ
        isScreen1Active = !isScreen1Active;
        UpdateScreen(isScreen1Active);
    }

    private void UpdateScreen(bool showScreen1)
    {
        // ȭ�� Ȱ��ȭ/��Ȱ��ȭ ����
        screen1.SetActive(showScreen1);
        screen2.SetActive(!showScreen1);
    }
}
