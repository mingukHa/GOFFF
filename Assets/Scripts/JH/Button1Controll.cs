using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Button1Controll : MonoBehaviour
{
    public Valve valve; // Valve ��ũ��Ʈ�� �����ϱ� ���� ����

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // grab�� ���۵� �� ȣ��Ǵ� �̺�Ʈ
            grabInteractable.selectEntered.AddListener(OnButtonEntered);
            // grab�� ���� �� ȣ��Ǵ� �̺�Ʈ
            grabInteractable.selectExited.AddListener(OnButtonReleased);
        }
    }

    private void OnButtonEntered(SelectEnterEventArgs arg0)
    {
        if (valve != null)
        {
            valve.StartRotation(); // ��� ȸ�� ����
        }
    }

    private void OnButtonReleased(SelectExitEventArgs arg0)
    {
        if (valve != null)
        {
            valve.StopRotation(); // ��� ȸ�� ����
        }
    }
}