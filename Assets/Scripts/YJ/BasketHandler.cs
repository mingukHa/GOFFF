using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField]
    private Transform basketTransform; // Basket의 Transform

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Grabbable") || collider.CompareTag("Key"))
        {
            XRGrabInteractable grabInteractable = collider.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab 중이 아닐 때만 처리
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
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab 중이 아닐 때만 처리
            {
                DetachFromBasket(collider.transform);
            }
        }
    }

    private void AttachToBasket(Transform grabbableObject)
    {
        // Grabbable Object의 부모를 Basket으로 설정
        grabbableObject.SetParent(basketTransform);

        // Rigidbody 설정 변경: 중력 비활성화, 휠체어와 함께 이동하도록 설정
        Rigidbody objectRigidbody = grabbableObject.GetComponent<Rigidbody>();
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true;
        }

        // Grab 해제 이벤트 처리
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener((args) => OnGrabReleased(grabbableObject));
        }
    }

    private void DetachFromBasket(Transform grabbableObject)
    {
        // Grabbable Object의 부모를 해제
        grabbableObject.SetParent(null);

        // Rigidbody 설정 복원
        Rigidbody objectRigidbody = grabbableObject.GetComponent<Rigidbody>();
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = false;
        }

        // Grab 해제 이벤트 제거
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener((args) => OnGrabReleased(grabbableObject));
        }
    }

    private void OnGrabReleased(Transform grabbableObject)
    {
        // Grab을 해제했을 때 Collider 상태 확인
        if (IsInsideBasket(grabbableObject))
        {
            // Basket 내부에 있다면 Attach 유지
            AttachToBasket(grabbableObject);
        }
        else
        {
            // Basket을 벗어났다면 Detach
            DetachFromBasket(grabbableObject);
        }
    }

    private bool IsInsideBasket(Transform grabbableObject)
    {
        // Basket 내부에 있는지 판단
        Collider objectCollider = grabbableObject.GetComponent<Collider>();
        Collider basketCollider = basketTransform.GetComponent<Collider>();

        if (objectCollider != null && basketCollider != null)
        {
            return basketCollider.bounds.Intersects(objectCollider.bounds);
        }
        return false;
    }
}
