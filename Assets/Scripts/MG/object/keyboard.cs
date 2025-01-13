using UnityEngine;

public class InputFieldClickHandler : MonoBehaviour
{
    public GameObject targetObject; // 활성화할 대상 오브젝트

    public void OnInputFieldClicked()
    {
        targetObject.SetActive(true); // 오브젝트 활성화
    }
}
