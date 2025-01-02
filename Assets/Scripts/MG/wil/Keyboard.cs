using UnityEngine;
using TMPro;

public class VirtualKeyboardController : MonoBehaviour
{
    private TMP_InputField targetInputField; // 현재 입력 대상 InputField

    // InputField 선택 시 호출 (OnSelect 이벤트와 연결)
    public void SetTargetInputField(TMP_InputField inputField)
    {
        if (inputField != null)
        {
            targetInputField = inputField; // 현재 입력 대상 설정
            Debug.Log($"가상 키보드의 대상 InputField가 {inputField.name}으로 설정되었습니다.");
        }
        else
        {
            Debug.LogError("입력된 InputField가 null입니다!");
        }
    }

    // 가상 키보드 버튼 클릭 시 호출
    public void OnKeyPress(string key)
    {
        if (targetInputField == null)
        {
            Debug.LogWarning("선택된 InputField가 없습니다!");
            return;
        }

        if (key == "BACKSPACE")
        {
            // Backspace 동작: 마지막 문자 삭제
            if (targetInputField.text.Length > 0)
            {
                targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
                Debug.Log($"현재 텍스트: {targetInputField.text}");
            }
        }
        else
        {
            // 일반 문자 입력
            targetInputField.text += key;
            Debug.Log($"현재 텍스트: {targetInputField.text}");
        }
    }
}
