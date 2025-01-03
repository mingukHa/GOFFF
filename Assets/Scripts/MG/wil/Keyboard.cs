using UnityEngine;
using TMPro; // TextMeshPro 관련 클래스 사용
using System.Collections.Generic;

public class KeyboardManager : MonoBehaviour
{
    public TMP_InputField keyboardInputField; // 가상 키보드의 InputField
    private TMP_InputField activeInputField;  // 현재 활성화된 대상 InputField
    private List<TMP_InputField> targetInputFields = new List<TMP_InputField>(); // 다중 대상 리스트

    void Start()
    {
        // 키보드 InputField 값 변경 이벤트 등록
        if (keyboardInputField != null)
        {
            keyboardInputField.onValueChanged.AddListener(UpdateTargetInputFields);
        }
    }

    /// <summary>
    /// 활성화된 대상 InputField를 설정
    /// </summary>
    /// <param name="newField">활성화할 InputField</param>
    public void SetActiveInputField(TMP_InputField newField)
    {
        activeInputField = newField;
        Debug.Log($"활성화된 InputField가 설정되었습니다: {newField.name}");
    }

    /// <summary>
    /// 대상 InputField 리스트에 추가
    /// </summary>
    /// <param name="newField">추가할 InputField</param>
    public void AddTargetInputField(TMP_InputField newField)
    {
        if (!targetInputFields.Contains(newField))
        {
            targetInputFields.Add(newField);
            Debug.Log($"InputField가 리스트에 추가되었습니다: {newField.name}");
        }
    }

    /// <summary>
    /// 대상 InputField 리스트에서 제거
    /// </summary>
    /// <param name="removeField">제거할 InputField</param>
    public void RemoveTargetInputField(TMP_InputField removeField)
    {
        if (targetInputFields.Contains(removeField))
        {
            targetInputFields.Remove(removeField);
            Debug.Log($"InputField가 리스트에서 제거되었습니다: {removeField.name}");
        }
    }

    /// <summary>
    /// 키보드 입력값을 활성화된 InputField에 복사
    /// </summary>
    public void CopyInputValueToActive()
    {
        if (keyboardInputField != null && activeInputField != null)
        {
            activeInputField.text = keyboardInputField.text;
            Debug.Log($"값이 복사되었습니다: {keyboardInputField.text}");
        }
        else
        {
            Debug.LogWarning("활성화된 InputField가 설정되지 않았습니다!");
        }
    }

    /// <summary>
    /// 키보드 입력값을 다중 대상 InputField로 실시간 업데이트
    /// </summary>
    /// <param name="inputValue">현재 키보드 입력값</param>
    private void UpdateTargetInputFields(string inputValue)
    {
        foreach (var inputField in targetInputFields)
        {
            inputField.text = inputValue;
        }
    }
}
