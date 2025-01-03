using System.Collections.Generic;
using UnityEngine;

public class PasswordPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] displaySlots; // 비밀번호를 표시할 슬롯 (왼쪽부터 3자리)
    [SerializeField] private string correctPassword = "123"; // 정답 비밀번호
    private List<int> inputPassword = new List<int>(); // 현재 입력된 비밀번호

    private void Start()
    {
        ClearPassword();
    }

    // 비밀번호 입력
    public void EnterNumber(int number)
    {
        if (inputPassword.Count < 3) // 최대 3자리까지만 입력 가능
        {
            inputPassword.Add(number);

            // 입력된 번호를 표시
            UpdateDisplay();
        }
    }

    // 입력 완료 (Enter 버튼)
    public void CheckPassword()
    {
        string currentPassword = string.Join("", inputPassword);

        if (currentPassword == correctPassword)
        {
            Debug.Log("문을 열어라.");
        }
        else
        {
            Debug.Log("답이 아닙니다.");
        }

        // 입력 초기화
        ClearPassword();
    }

    // 비밀번호를 초기화
    private void ClearPassword()
    {
        inputPassword.Clear();
        foreach (GameObject slot in displaySlots)
        {
            // 슬롯에 아무것도 표시되지 않도록 초기화
            if (slot.TryGetComponent(out TMPro.TextMeshProUGUI text))
            {
                text.text = "";
            }
        }
    }

    // 입력된 비밀번호를 표시
    private void UpdateDisplay()
    {
        for (int i = 0; i < displaySlots.Length; i++)
        {
            if (i < inputPassword.Count && displaySlots[i].TryGetComponent(out TMPro.TextMeshProUGUI text))
            {
                text.text = inputPassword[i].ToString();
            }
        }
    }
}
