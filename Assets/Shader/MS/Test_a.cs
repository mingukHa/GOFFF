// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;


public class Test_a : MonoBehaviour
{
    Test_Parent par = null;
    private bool isBPressed = false;
    private bool wasBPressed = false; // ��ư�� ���������� ���� ���� ����

    private void Start()
    {
        par = GetComponentInParent<Test_Parent>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray mousePosToRay = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;
            if (Physics.Raycast(mousePosToRay, out hit))
            {
                par.StartSonarRing(hit.point, 1.4f, 0);
            }
        }
    }
    //private void Update()
    //{
    //    // ������ ��Ʈ�ѷ��� �Է� ���� ��������
    //    InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    //    // Secondary Button (B ��ư) ���� �б�
    //    if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
    //    {
    //        if (!wasBPressed)
    //        {
    //            // B ��ư�� ������ �� ����
    //            RaycastHit hit;
    //            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                par.StartSonarRing(hit.point, 1.4f, 0);
    //            }
    //            wasBPressed = true;
    //        }
    //    }
    //    else
    //        wasBPressed = false;
    //}

    void OnCollisionEnter(Collision collision)
    {
        // Start sonar ring from the contact point
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
