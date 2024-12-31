using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public SocketValidator[] socketValidators; // 도플갱어 방에 있는 모든 SocketValidator
    public GameObject jailBar; // 감옥 쇠창살 오브젝트

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
