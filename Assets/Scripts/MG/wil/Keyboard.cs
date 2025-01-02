using UnityEngine;
using TMPro;

public class MoveInputFieldValue : MonoBehaviour
{
    [SerializeField] private TMP_InputField sourceInputField; // ���� ������ InputField
    [SerializeField] private TMP_InputField[] targetInputFields; // ���� ���� InputField

    private void Start()
    {
        // Source InputField�� Enter Ű ���� �̺�Ʈ �߰�
        if (sourceInputField != null)
        {
            sourceInputField.onSubmit.AddListener(OnSubmit); // Enter Ű ������ �� �̺�Ʈ ����
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� (�޸� ���� ����)
        if (sourceInputField != null)
        {
            sourceInputField.onSubmit.RemoveListener(OnSubmit);
        }
    }

    // Enter Ű�� ������ ȣ��Ǵ� �޼���
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
            sourceInputField.text = ""; // Source �ʱ�ȭ
        }
    }
}
