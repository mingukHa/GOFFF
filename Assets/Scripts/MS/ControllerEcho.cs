using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class ControllerEcho : MonoBehaviourPun
{
    private bool isBPressed = false;
    private bool wasBPressed = false; // ��ư�� ���������� ���� ���� ����
    SimpleSonarShader_Parent par = null;

    private void Start()
    {
        par = GetComponent<SimpleSonarShader_Parent>();
    }
    private void Update()
    {
        // ������ ��Ʈ�ѷ��� �Է� ���� ��������
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Secondary Button (B ��ư) ���� �б�
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
        {
            Debug.Log("�ĵ� �Է��� �ް� ����");
            if (!wasBPressed)
            {
                // B ��ư�� ������ �� ����
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                if (Physics.Raycast(ray, out hit))
                {
                    par.StartSonarRing(hit.point, 1.4f, 0);
                    Debug.Log($"{hit.point}�Դϴ�");
                }
                wasBPressed = true;
            }
        }
        else
            wasBPressed = false;
    }
}
