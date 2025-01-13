using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldSelector : MonoBehaviour
{
    [Header("Default Output Field")]
    [SerializeField] private TMP_InputField defaultOutputField;

    private TMP_InputField currentOutputField;

    private void Awake()
    {
        // �ʱ�ȭ
        currentOutputField = defaultOutputField;
    }

    public void OnInputFieldSelected(TMP_InputField selectedField)
    {
        // ���õ� InputField�� �������� ����
        currentOutputField = selectedField;
    }

    public TMP_InputField GetCurrentOutputField()
    {
        return currentOutputField;
    }
}

