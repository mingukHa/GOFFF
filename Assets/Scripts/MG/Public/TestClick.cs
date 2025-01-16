using UnityEngine;

public class DropObjectOnClick : MonoBehaviour
{
    public GameObject objectToDrop; // ����߸� ��ü ������
    public float dropHeight = 10f; // ��ü�� ������ ����

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Ŭ���� ��ġ�� ��ǥ
                Vector3 dropPosition = hit.point;
                dropPosition.y += dropHeight; // ������ ���̿��� ���������� ����

                // ��ü ����
                Instantiate(objectToDrop, dropPosition, Quaternion.identity);
            }
        }
    }
}
