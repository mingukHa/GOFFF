using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public SocketValidator[] socketValidators; // ���ð��� �濡 �ִ� ��� SocketValidator
    public GameObject jailBar; // ���� ��â�� ������Ʈ

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
    }
}
