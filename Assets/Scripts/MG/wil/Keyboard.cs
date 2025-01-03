using UnityEngine;
using TMPro; // TextMeshPro ���� Ŭ���� ���
using System.Collections.Generic;

public class KeyboardManager : MonoBehaviour
{
    public TMP_InputField keyboardInputField; // ���� Ű������ InputField
    private TMP_InputField activeInputField;  // ���� Ȱ��ȭ�� ��� InputField
    private List<TMP_InputField> targetInputFields = new List<TMP_InputField>(); // ���� ��� ����Ʈ

    void Start()
    {
        // Ű���� InputField �� ���� �̺�Ʈ ���
        if (keyboardInputField != null)
        {
            keyboardInputField.onValueChanged.AddListener(UpdateTargetInputFields);
        }
    }

    /// <summary>
    /// Ȱ��ȭ�� ��� InputField�� ����
    /// </summary>
    /// <param name="newField">Ȱ��ȭ�� InputField</param>
    public void SetActiveInputField(TMP_InputField newField)
    {
        activeInputField = newField;
        Debug.Log($"Ȱ��ȭ�� InputField�� �����Ǿ����ϴ�: {newField.name}");
    }

    /// <summary>
    /// ��� InputField ����Ʈ�� �߰�
    /// </summary>
    /// <param name="newField">�߰��� InputField</param>
    public void AddTargetInputField(TMP_InputField newField)
    {
        if (!targetInputFields.Contains(newField))
        {
            targetInputFields.Add(newField);
            Debug.Log($"InputField�� ����Ʈ�� �߰��Ǿ����ϴ�: {newField.name}");
        }
    }

    /// <summary>
    /// ��� InputField ����Ʈ���� ����
    /// </summary>
    /// <param name="removeField">������ InputField</param>
    public void RemoveTargetInputField(TMP_InputField removeField)
    {
        if (targetInputFields.Contains(removeField))
        {
            targetInputFields.Remove(removeField);
            Debug.Log($"InputField�� ����Ʈ���� ���ŵǾ����ϴ�: {removeField.name}");
        }
    }

    /// <summary>
    /// Ű���� �Է°��� Ȱ��ȭ�� InputField�� ����
    /// </summary>
    public void CopyInputValueToActive()
    {
        if (keyboardInputField != null && activeInputField != null)
        {
            activeInputField.text = keyboardInputField.text;
            Debug.Log($"���� ����Ǿ����ϴ�: {keyboardInputField.text}");
        }
        else
        {
            Debug.LogWarning("Ȱ��ȭ�� InputField�� �������� �ʾҽ��ϴ�!");
        }
    }

    /// <summary>
    /// Ű���� �Է°��� ���� ��� InputField�� �ǽð� ������Ʈ
    /// </summary>
    /// <param name="inputValue">���� Ű���� �Է°�</param>
    private void UpdateTargetInputFields(string inputValue)
    {
        foreach (var inputField in targetInputFields)
        {
            inputField.text = inputValue;
        }
    }
}
