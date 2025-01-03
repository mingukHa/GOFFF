using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabValve : MonoBehaviour
{
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;

    private bool isGrabed = false;


    // �ٸ� Collider�� �� ���� �浹���� �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if(!isGrabed)
            grabValveTrigger?.Invoke(gameObject,other);
    }

    public void SelectOn()
    {
        Debug.Log("��긦 ��ҽ��ϴ�.");
        isGrabed = true;
    }

    public void SelectOff()
    {
        Debug.Log("��긦 ���ҽ��ϴ�.");
        isGrabed = false;
    }

}
