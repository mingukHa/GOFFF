using UnityEngine;

public class InputFieldClickHandler : MonoBehaviour
{
    public GameObject targetObject; // Ȱ��ȭ�� ��� ������Ʈ

    public void OnInputFieldClicked()
    {
        targetObject.SetActive(true); // ������Ʈ Ȱ��ȭ
    }
}
