using UnityEngine;

public class PasswordEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] numbtn;   //���ڹ�ư List
    [SerializeField] private PasswordPuzzle passwordPuzzle;

    private void Start()
    {
        // ���� ��ư�� ��ȣ ���� �� ���� ����
        for (int i = 0; i < numbtn.Length; i++)
        {
            int buttonIndex = i; // ���� ������ ��ư �ε��� ����
            {
                if (buttonIndex < 10) // 0~9 ���� ��ư
                {
                    NumDisplayed(buttonIndex);
                }
                else if (buttonIndex == 10) // Enter ��ư
                {
                    NumDisplayedEnter();
                }
            }
        }
    }

    public void NumDisplayed(int buttonIndex)
    {
        if (passwordPuzzle != null)
        {
            passwordPuzzle.EnterNumber(buttonIndex);
        }
    }

    public void NumDisplayedEnter()
    {
        if (passwordPuzzle != null)
        {
            passwordPuzzle.CheckPassword();
        }
    }
}