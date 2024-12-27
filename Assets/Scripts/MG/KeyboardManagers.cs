using UnityEngine;
using TMPro;

public class VRKeyboardManager : MonoBehaviour
{
    public GameObject virtualKeyboard; // ���� Ű���� ������Ʈ
    private TMP_InputField activeInputField; // ���� Ȱ��ȭ�� TMP_InputField

    // TMP_InputField Ŭ�� �� ȣ���Ͽ� ���� Ű���带 Ȱ��ȭ
    public void ShowKeyboard(TMP_InputField inputField)
    {
        activeInputField = inputField; // ���� Ȱ��ȭ�� InputField ����
        virtualKeyboard.SetActive(true); // ���� Ű���� Ȱ��ȭ
        activeInputField.text = ""; // �ʱ�ȭ (�ʿ��)
    }

    // ���� Ű���忡�� ���� �Է�
    public void AddCharacter(string character)
    {
        if (activeInputField != null)
        {
            activeInputField.text += character; // TMP_InputField �ؽ�Ʈ ������Ʈ
        }
    }

    // Backspace ó��
    public void Backspace()
    {
        if (activeInputField != null && activeInputField.text.Length > 0)
        {
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
        }
    }

    // Enter Ű ó��
    public void SubmitInput()
    {
        if (activeInputField != null)
        {
            Debug.Log($"Final Input: {activeInputField.text}"); // ���� �ؽ�Ʈ ��� (�ʿ� �� �ٸ� ���� �߰�)
        }
        CloseKeyboard();
    }

    // ���� Ű���� �ݱ�
    public void CloseKeyboard()
    {
        virtualKeyboard.SetActive(false); // ���� Ű���� ��Ȱ��ȭ
        activeInputField = null; // Ȱ��ȭ�� InputField �ʱ�ȭ
    }
}




