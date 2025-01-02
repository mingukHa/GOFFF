using UnityEngine;
using TMPro;

public class MoveInputFieldValue : MonoBehaviour
{
    [SerializeField] private TMP_InputField sourceInputField; // 값을 가져올 InputField
    [SerializeField] private TMP_InputField[] targetInputFields; // 값을 보낼 InputField

    private void Start()
    {
        // Source InputField에 Enter 키 감지 이벤트 추가
        if (sourceInputField != null)
        {
            sourceInputField.onSubmit.AddListener(OnSubmit); // Enter 키 눌렀을 때 이벤트 실행
        }
    }

    private void OnDestroy()
    {
        // 이벤트 제거 (메모리 누수 방지)
        if (sourceInputField != null)
        {
            sourceInputField.onSubmit.RemoveListener(OnSubmit);
        }
    }

    // Enter 키를 누르면 호출되는 메서드
    private void OnSubmit(string inputText)
    {
        foreach (TMP_InputField target in targetInputFields)
        {
            if (target != null)
            {
                target.text = inputText;
            }
        }
        sourceInputField.text = "";
    }
    public void MoveValue()
    {
        foreach (TMP_InputField target in targetInputFields)
        {
            target.text = sourceInputField.text;
            sourceInputField.text = ""; // Source 초기화
        }
    }
}
