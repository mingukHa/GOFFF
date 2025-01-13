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
        // 초기화
        currentOutputField = defaultOutputField;
    }

    public void OnInputFieldSelected(TMP_InputField selectedField)
    {
        // 선택된 InputField를 동적으로 변경
        currentOutputField = selectedField;
    }

    public TMP_InputField GetCurrentOutputField()
    {
        return currentOutputField;
    }
}

