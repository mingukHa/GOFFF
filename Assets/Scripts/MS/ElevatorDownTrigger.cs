using System;
using UnityEngine;

public class ElevatorDownTrigger : MonoBehaviour
{
    // ��������Ʈ ����
    public Action<Collider> OnPlayerTriggered;

    private Collider Player;

    private void OnTriggerEnter(Collider other)
    {
        // "Player" �±����� Ȯ��
        if (other.CompareTag("Player"))
        {
            if (Player == null)
            {
                // ù ��° �÷��̾� ���
                Player = other;

                // �� �÷��̾ ��� Ʈ���� �ȿ� ���� �� ��������Ʈ ����
                OnPlayerTriggered?.Invoke(Player);

                // �ʱ�ȭ (���� ����)
                ResetPlayers();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ Ʈ���Ÿ� ������ �ʱ�ȭ
        if (other == Player)
        {
            Player = null;
        }
    }

    // �ʱ�ȭ �Լ�
    private void ResetPlayers()
    {
        Player = null;
    }
}
