using UnityEngine;

public class Valve : MonoBehaviour
{
    public Transform cylinderAttachPoint; // Cylinder의 벨브가 붙을 위치 (Cylinder에 빈 GameObject 추가 필요)

    private Rigidbody valveRigidbody;

    void Start()
    {
        valveRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cylinder")) // Cylinder에 태그를 "Cylinder"로 설정
        {
            AttachToCylinder(collision.gameObject);
        }
    }

    void AttachToCylinder(GameObject cylinder)
    {
        // Rigidbody 비활성화
        valveRigidbody.isKinematic = true;
        valveRigidbody.useGravity = false;

        // 벨브 위치와 회전 설정
        Transform attachPoint = cylinder.transform.Find("AttachPoint"); // Cylinder에 AttachPoint를 자식으로 설정
        if (attachPoint != null)
        {
            transform.position = attachPoint.position;
            transform.rotation = attachPoint.rotation;
        }
        else
        {
            Debug.LogWarning("AttachPoint가 Cylinder에 없습니다.");
        }
    }
}