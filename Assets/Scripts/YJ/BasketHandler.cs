using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField]
    private Transform basketTransform; // Basket�� Transform

    private void OnTriggerEnter(Collider other)
    {
        // Grabbable Object�� Basket�� ���� �� ó��
        if (other.CompareTag("Grabbable"))
        {
            // Grabbable Object�� �θ� Basket���� ����
            other.transform.SetParent(basketTransform);

            // Rigidbody ���� ����: �߷� ��Ȱ��ȭ, ��ü��� �Բ� �̵��ϵ��� ����
            Rigidbody objectRigidbody = other.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = true;
            }

            // Grabbable Object�� Grab �̺�Ʈ ����
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener((args) => DetachFromBasket(other.transform));
            }
        }
    }

    private void DetachFromBasket(Transform grabbableObject)
    {
        // Grabbable Object�� �θ� ����
        grabbableObject.SetParent(null);

        // Rigidbody ���� ����
        Rigidbody objectRigidbody = grabbableObject.GetComponent<Rigidbody>();
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = false;
        }

        // Grab �̺�Ʈ ���� ����
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveAllListeners();
        }
    }
}
