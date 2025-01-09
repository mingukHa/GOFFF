using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TVScreenToggle : MonoBehaviour
{
    public GameObject screen1; // Screen 1
    public GameObject screen2; // Screen 2
    private bool isScreen1Active = true; // Screen 1의 초기 활성 상태

    private void Start()
    {
        // 초기 상태 설정
        screen1.SetActive(true);
        screen2.SetActive(false);
    }

    public void ToggleScreen()
    {
        // 활성 상태 전환
        isScreen1Active = !isScreen1Active;
        screen1.SetActive(isScreen1Active);
        screen2.SetActive(!isScreen1Active);
    }
}
