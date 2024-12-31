// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;
using UnityEngine.XR;


public class SimpleSonarShader_ExampleCollision : MonoBehaviour
{
    SimpleSonarShader_Parent par = null;

    private void Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();
    }
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        Ray mousePosToRay = Camera.main.ScreenPointToRay(mousePos);

    //        RaycastHit hit;
    //        if (Physics.Raycast(mousePosToRay, out hit))
    //        {
    //            par.StartSonarRing(hit.point, 1.4f, 0);
    //        }
    //    }
    //}
    private void Update()
    {
        // 오른손 컨트롤러의 입력 상태 가져오기
        InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool isBPressed = false;

        // Secondary Button (B 버튼) 상태 읽기
        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isBPressed) && isBPressed)
        {
            // B 버튼이 눌렸을 때 동작
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out hit))
            {
                par.StartSonarRing(hit.point, 1.4f, 0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Start sonar ring from the contact point
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
