using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputFieldEvents : MonoBehaviour, ISelectHandler
{
    [SerializeField] private InputFieldSelector inputFieldSelector;

    public void OnSelect(BaseEventData eventData)
    {
        inputFieldSelector.OnInputFieldSelected(GetComponent<TMP_InputField>());
    }
}
