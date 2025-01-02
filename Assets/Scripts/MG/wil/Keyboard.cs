using UnityEngine;
using TMPro;

public class VirtualKeyboardController : MonoBehaviour
{
    private TMP_InputField targetInputField; // ���� �Է� ��� InputField

    // InputField ���� �� ȣ�� (OnSelect �̺�Ʈ�� ����)
    public void SetTargetInputField(TMP_InputField inputField)
    {
        if (inputField != null)
        {
            targetInputField = inputField; // ���� �Է� ��� ����
            Debug.Log($"���� Ű������ ��� InputField�� {inputField.name}���� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�Էµ� InputField�� null�Դϴ�!");
        }
    }

    // ���� Ű���� ��ư Ŭ�� �� ȣ��
    public void OnKeyPress(string key)
    {
        if (targetInputField == null)
        {
            Debug.LogWarning("���õ� InputField�� �����ϴ�!");
            return;
        }

        if (key == "BACKSPACE")
        {
            // Backspace ����: ������ ���� ����
            if (targetInputField.text.Length > 0)
            {
                targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
                Debug.Log($"���� �ؽ�Ʈ: {targetInputField.text}");
            }
        }
        else
        {
            // �Ϲ� ���� �Է�
            targetInputField.text += key;
            Debug.Log($"���� �ؽ�Ʈ: {targetInputField.text}");
        }
    }
}
