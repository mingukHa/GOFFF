using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Waitscene gameManager; // ���� �Ŵ��� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾����� Ȯ��
        {
            gameManager.UpdateReadyCount(1); // ���� �Ŵ����� �÷��̾� �غ� ���� ����
            Debug.Log("���� Ȱ��ȭ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾����� Ȯ��
        {
            gameManager.UpdateReadyCount(-1); // ���� �Ŵ����� �÷��̾� �غ� ���� ����
            Debug.Log("���� ����");
        }
    }
}
