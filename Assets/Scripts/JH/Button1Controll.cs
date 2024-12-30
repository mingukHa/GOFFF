using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Button1Controll : MonoBehaviour
{
    public Valve valve; // Valve 스크립트를 참조하기 위한 변수

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // grab이 시작될 때 호출되는 이벤트
            grabInteractable.selectEntered.AddListener(OnButtonEntered);
            // grab이 끝날 때 호출되는 이벤트
            grabInteractable.selectExited.AddListener(OnButtonReleased);
        }
    }

    private void OnButtonEntered(SelectEnterEventArgs arg0)
    {
        if (valve != null)
        {
            valve.StartRotation(); // 밸브 회전 시작
        }
    }

    private void OnButtonReleased(SelectExitEventArgs arg0)
    {
        if (valve != null)
        {
            valve.StopRotation(); // 밸브 회전 멈춤
        }
    }
}