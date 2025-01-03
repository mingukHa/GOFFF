using UnityEngine;

public class PasswordEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] numbtn;   //숫자버튼 List
    [SerializeField] private PasswordPuzzle passwordPuzzle;

    private void Start()
    {
        // 숫자 버튼에 번호 정의 및 동작 설정
        for (int i = 0; i < numbtn.Length; i++)
        {
            int buttonIndex = i; // 지역 변수로 버튼 인덱스 저장
            {
                if (buttonIndex < 10) // 0~9 숫자 버튼
                {
                    NumDisplayed(buttonIndex);
                }
                else if (buttonIndex == 10) // Enter 버튼
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