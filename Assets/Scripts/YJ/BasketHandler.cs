using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BasketHandler : MonoBehaviour
{
    [SerializeField] private Transform basketTransform; // Basket의 Transform

    private void OnTriggerEnter(Collider other)
    {
        // Grabbable Object가 Basket에 들어올 때 처리
        if (other.CompareTag("Grabbable"))
        {
            // Grab 상태 확인
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab 중이 아니어야 함
            {
                // Grabbable Object의 부모를 Basket으로 설정
                other.transform.SetParent(basketTransform);

                // Rigidbody 설정 변경: 중력 비활성화, 휠체어와 함께 이동하도록 설정
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
        // Grabbable Object가 Basket에서 나갈 때 처리
        if (other.CompareTag("Grabbable"))
        {
            // Grab 상태 확인
            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null && !grabInteractable.isSelected) // Grab 중이 아니어야 함
            {
                // Grabbable Object의 부모를 해제
                other.transform.SetParent(null);

                // Rigidbody 설정 복원
                Rigidbody objectRigidbody = other.GetComponent<Rigidbody>();
                if (objectRigidbody != null)
                {
                    objectRigidbody.isKinematic = false;
                }
            }
        }
    }
}
