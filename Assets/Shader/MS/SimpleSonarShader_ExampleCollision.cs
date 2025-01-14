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

    void OnCollisionEnter(Collision collision)
    {
        // Start sonar ring from the contact point
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
