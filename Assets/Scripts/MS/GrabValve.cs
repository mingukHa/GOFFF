using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabValve : MonoBehaviour
{
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;

    private bool isGrabed = false;


    // 다른 Collider가 이 밸브와 충돌했을 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if(!isGrabed)
            grabValveTrigger?.Invoke(gameObject,other);
    }

    public void SelectOn()
    {
        Debug.Log("밸브를 잡았습니다.");
        isGrabed = true;
    }

    public void SelectOff()
    {
        Debug.Log("밸브를 놓았습니다.");
        isGrabed = false;
    }

}
