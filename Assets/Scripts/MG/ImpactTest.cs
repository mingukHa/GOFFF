using UnityEngine;

public class ImpactTest : MonoBehaviour
{
    public GameObject targetObject; // �浹 �׽�Ʈ�� ��� ��ü

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            // ȭ�鿡�� Ŭ���� ��ġ�� Ray�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Ŭ���� ��ġ�� ��ü �̵�
                Vector3 clickPosition = hit.point;
                targetObject.transform.position = clickPosition + Vector3.up;

                // �浹 Ȯ��
                Collider[] hitColliders = Physics.OverlapSphere(clickPosition, 0.1f);
                foreach (Collider collider in hitColliders)
                {
                    if (collider.gameObject != targetObject)
                    {
                        //Debug.Log("�浹: " + collider.name);
                    }
                }
            }
            
        }
        
        
    }
}

