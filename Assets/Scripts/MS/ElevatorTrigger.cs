using System;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    // ��������Ʈ ����
    public Action<Collider, Collider> OnPlayersTriggered;

    private Collider firstPlayer;
    private Collider secondPlayer;

    private void OnTriggerEnter(Collider other)
    {
        // "Player" �±����� Ȯ��
        if (other.CompareTag("Player"))
        {
            if (firstPlayer == null)
            {
                // ù ��° �÷��̾� ���
                firstPlayer = other;
            }
            else if (secondPlayer == null && other.name != firstPlayer.name)
            {
                // �� ��° �÷��̾� ���
                secondPlayer = other;

                // �� �÷��̾ ��� Ʈ���� �ȿ� ���� �� ��������Ʈ ����
                OnPlayersTriggered?.Invoke(firstPlayer, secondPlayer);

                // �ʱ�ȭ (���� ����)
                ResetPlayers();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �÷��̾ Ʈ���Ÿ� ������ �ʱ�ȭ
        if (other == firstPlayer)
        {
            firstPlayer = null;
        }
        else if (other == secondPlayer)
        {
            secondPlayer = null;
        }
    }

    // �ʱ�ȭ �Լ�
    private void ResetPlayers()
    {
        firstPlayer = null;
        secondPlayer = null;
    }
}
