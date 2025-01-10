using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField] private Transform basketTransform; // Basket�� Transform

    private void OnTriggerEnter(Collider other)
    {
        // Grabbable Object�� Basket�� ���� �� ó��
        if (other.CompareTag("Grabbable"))
        {
            // Grab ���� Ȯ��
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab ���� �ƴϾ�� ��
            {
                // Grabbable Object�� �θ� Basket���� ����
                other.transform.SetParent(basketTransform);

                // Rigidbody ���� ����: �߷� ��Ȱ��ȭ, ��ü��� �Բ� �̵��ϵ��� ����
                Rigidbody objectRigidbody = other.GetComponent<Rigidbody>();
                if (objectRigidbody != null)
                {
                    objectRigidbody.isKinematic = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Grabbable Object�� Basket���� ���� �� ó��
        if (other.CompareTag("Grabbable"))
        {
            // Grab ���� Ȯ��
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab ���� �ƴϾ�� ��
            {
                // Grabbable Object�� �θ� ����
                other.transform.SetParent(null);

                // Rigidbody ���� ����
                Rigidbody objectRigidbody = other.GetComponent<Rigidbody>();
                if (objectRigidbody != null)
                {
                    objectRigidbody.isKinematic = false;
                }
            }
        }
    }
}
