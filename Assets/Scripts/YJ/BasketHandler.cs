using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField]
    private Transform basketTransform; // Basket�� Transform

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Grabbable") || collider.CompareTag("Key"))
        {
            XRGrabInteractable grabInteractable = collider.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab ���� �ƴ� ���� ó��
            {
                AttachToBasket(collider.transform);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Grabbable") || collider.CompareTag("Key"))
        {
            XRGrabInteractable grabInteractable = collider.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab ���� �ƴ� ���� ó��
            {
                DetachFromBasket(collider.transform);
            }
        }
    }

    private void AttachToBasket(Transform grabbableObject)
    {
        // Grabbable Object�� �θ� Basket���� ����
        grabbableObject.SetParent(basketTransform);

        // Rigidbody ���� ����: �߷� ��Ȱ��ȭ, ��ü��� �Բ� �̵��ϵ��� ����
        Rigidbody objectRigidbody = grabbableObject.GetComponent<Rigidbody>();
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
        }

        // Grab ���� �̺�Ʈ ó��
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener((args) => OnGrabReleased(grabbableObject));
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

        // Grab ���� �̺�Ʈ ����
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener((args) => OnGrabReleased(grabbableObject));
        }
    }

    private void OnGrabReleased(Transform grabbableObject)
    {
        // Grab�� �������� �� Collider ���� Ȯ��
        if (IsInsideBasket(grabbableObject))
        {
            // Basket ���ο� �ִٸ� Attach ����
            AttachToBasket(grabbableObject);
        }
        else
        {
            // Basket�� ����ٸ� Detach
            DetachFromBasket(grabbableObject);
        }
    }

    private bool IsInsideBasket(Transform grabbableObject)
    {
        // Basket ���ο� �ִ��� �Ǵ�
        Collider objectCollider = grabbableObject.GetComponent<Collider>();
        Collider basketCollider = basketTransform.GetComponent<Collider>();

        if (objectCollider != null && basketCollider != null)
        {
            return basketCollider.bounds.Intersects(objectCollider.bounds);
        }
        return false;
    }
}
