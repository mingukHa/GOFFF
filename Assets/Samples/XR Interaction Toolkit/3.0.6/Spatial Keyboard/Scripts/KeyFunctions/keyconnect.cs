using UnityEngine;
using UnityEngine.EventSystems; // 이벤트 시스템을 사용하기 위해 필요
using TMPro; // TMP_InputField 지원
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
            // 자동 연결 활성화 상태인지 확인
            if (m_AutoLinkToSelectedInputField)
            {
                // 현재 선택된 GameObject를 가져옴
                var selectedObject = EventSystem.current.currentSelectedGameObject;

                // 선택된 오브젝트가 TMP_InputField인지 확인
                if (selectedObject != null && selectedObject.TryGetComponent(out TMP_InputField inputField))
                {
                    // 선택된 InputField와 현재 연동된 InputField가 다르면 연동
                    if (inputField != m_CurrentInputField)
                    {
                        ConnectToInputField(inputField);
                    }
                }
            }
        }

        void ConnectToInputField(TMP_InputField inputField)
        {
            // 기존 InputField의 리스너 제거
            if (m_CurrentInputField != null)
            {
                m_CurrentInputField.onSelect.RemoveListener(OnInputFieldSelected);
            }

            // 새 InputField를 설정하고 리스너 추가
            m_CurrentInputField = inputField;
            m_CurrentInputField.onSelect.AddListener(OnInputFieldSelected);

            Debug.Log($"Connected to InputField: {m_CurrentInputField.name}");

            // 키보드와 새 InputField를 연동
            m_Keyboard?.Open(m_CurrentInputField);
        }

        void OnInputFieldSelected(string text)
        {
            Debug.Log($"InputField {m_CurrentInputField.name} selected with text: {text}");

            // 키보드 연동 처리 (이미 연동된 상태일 가능성이 높음)
            if (m_Keyboard != null)
            {
                m_Keyboard.Open(m_CurrentInputField);
            }
        }

        void OnDisable()
        {
            // 기존 InputField 리스너 제거
            if (m_CurrentInputField != null)
            {
                m_CurrentInputField.onSelect.RemoveListener(OnInputFieldSelected);
            }
        }
    }
}

