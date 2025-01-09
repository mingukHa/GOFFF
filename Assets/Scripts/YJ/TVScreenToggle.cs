using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TVScreenToggle : MonoBehaviour
{
    public GameObject screen1; // Screen 1
    public GameObject screen2; // Screen 2
    private bool isScreen1Active = true; // Screen 1�� �ʱ� Ȱ�� ����

    private void Start()
    {
        // �ʱ� ���� ����
        screen1.SetActive(true);
        screen2.SetActive(false);
    }

    public void ToggleScreen()
    {
        // Ȱ�� ���� ��ȯ
        isScreen1Active = !isScreen1Active;
        screen1.SetActive(isScreen1Active);
        screen2.SetActive(!isScreen1Active);
    }
}
