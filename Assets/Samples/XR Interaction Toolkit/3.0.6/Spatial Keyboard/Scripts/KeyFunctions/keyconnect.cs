using UnityEngine;
using UnityEngine.EventSystems; // �̺�Ʈ �ý����� ����ϱ� ���� �ʿ�
using TMPro; // TMP_InputField ����
using UnityEngine.Events;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard
{
    public class XRKeyboardDisplay2 : MonoBehaviour
    {
        [SerializeField, Tooltip("Default keyboard reference if no specific input field is selected.")]
        XRKeyboard m_Keyboard;

        [SerializeField, Tooltip("If true, keyboard will automatically connect to currently selected input field.")]
        bool m_AutoLinkToSelectedInputField = true;

        TMP_InputField m_CurrentInputField;

        void Update()
        {
            // �ڵ� ���� Ȱ��ȭ �������� Ȯ��
            if (m_AutoLinkToSelectedInputField)
            {
                // ���� ���õ� GameObject�� ������
                var selectedObject = EventSystem.current.currentSelectedGameObject;

                // ���õ� ������Ʈ�� TMP_InputField���� Ȯ��
                if (selectedObject != null && selectedObject.TryGetComponent(out TMP_InputField inputField))
                {
                    // ���õ� InputField�� ���� ������ InputField�� �ٸ��� ����
                    if (inputField != m_CurrentInputField)
                    {
                        ConnectToInputField(inputField);
                    }
                }
            }
        }

        void ConnectToInputField(TMP_InputField inputField)
        {
            // ���� InputField�� ������ ����
            if (m_CurrentInputField != null)
            {
                m_CurrentInputField.onSelect.RemoveListener(OnInputFieldSelected);
            }

            // �� InputField�� �����ϰ� ������ �߰�
            m_CurrentInputField = inputField;
            m_CurrentInputField.onSelect.AddListener(OnInputFieldSelected);

            Debug.Log($"Connected to InputField: {m_CurrentInputField.name}");

            // Ű����� �� InputField�� ����
            m_Keyboard?.Open(m_CurrentInputField);
        }

        void OnInputFieldSelected(string text)
        {
            Debug.Log($"InputField {m_CurrentInputField.name} selected with text: {text}");

            // Ű���� ���� ó�� (�̹� ������ ������ ���ɼ��� ����)
            if (m_Keyboard != null)
            {
                m_Keyboard.Open(m_CurrentInputField);
            }
        }

        void OnDisable()
        {
            // ���� InputField ������ ����
            if (m_CurrentInputField != null)
            {
                m_CurrentInputField.onSelect.RemoveListener(OnInputFieldSelected);
            }
        }
    }
}

