using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField]
    private Transform basketTransform; // Basket의 Transform

    private void OnTriggerEnter(Collider other)
    {
        // Grabbable Object가 Basket에 들어올 때 처리
        if (other.CompareTag("Grabbable"))
        {
            // Grabbable Object의 부모를 Basket으로 설정
            other.transform.SetParent(basketTransform);

            // Rigidbody 설정 변경: 중력 비활성화, 휠체어와 함께 이동하도록 설정
            Rigidbody objectRigidbody = other.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = true;
            }

            // Grabbable Object의 Grab 이벤트 구독
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener((args) => DetachFromBasket(other.transform));
            }
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

        // Grab 이벤트 구독 해제
        XRGrabInteractable grabInteractable = grabbableObject.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveAllListeners();
        }
    }
}
