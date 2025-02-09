﻿// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;


public class SimpleSonarShader_ExampleCollision : MonoBehaviourPun
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


    void OnCollisionEnter(Collision collision)
    {
        // Start sonar ring from the contact point
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
