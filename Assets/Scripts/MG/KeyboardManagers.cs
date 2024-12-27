using UnityEngine;
using TMPro;

public class VRKeyboardManager : MonoBehaviour
{
    public GameObject virtualKeyboard; // 가상 키보드 오브젝트
    private TMP_InputField activeInputField; // 현재 활성화된 TMP_InputField

    // TMP_InputField 클릭 시 호출하여 가상 키보드를 활성화
    public void ShowKeyboard(TMP_InputField inputField)
    {
        activeInputField = inputField; // 현재 활성화된 InputField 저장
        virtualKeyboard.SetActive(true); // 가상 키보드 활성화
        activeInputField.text = ""; // 초기화 (필요시)
    }

    // 가상 키보드에서 문자 입력
    public void AddCharacter(string character)
    {
        if (activeInputField != null)
        {
            activeInputField.text += character; // TMP_InputField 텍스트 업데이트
        }
    }

    // Backspace 처리
    public void Backspace()
    {
        if (activeInputField != null && activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
        }
    }

    // Enter 키 처리
    public void SubmitInput()
    {
        if (activeInputField != null)
        {
            Debug.Log($"Final Input: {activeInputField.text}"); // 최종 텍스트 출력 (필요 시 다른 동작 추가)
        }
        CloseKeyboard();
    }

    // 가상 키보드 닫기
    public void CloseKeyboard()
    {
        virtualKeyboard.SetActive(false); // 가상 키보드 비활성화
        activeInputField = null; // 활성화된 InputField 초기화
    }
}




