// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;


public class Test_a : MonoBehaviour
{
    Test_Parent par = null;
    private bool isBPressed = false;
    private bool wasBPressed = false; // 버튼이 마지막으로 눌린 상태 추적

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
    //    // 오른손 컨트롤러의 입력 상태 가져오기
    //    InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    //    // Secondary Button (B 버튼) 상태 읽기
    //    if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
    //    {
    //        if (!wasBPressed)
    //        {
    //            // B 버튼이 눌렸을 때 동작
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
