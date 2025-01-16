using UnityEngine;

public class DropObjectOnClick : MonoBehaviour
{
    public GameObject objectToDrop; // 떨어뜨릴 물체 프리팹
    public float dropHeight = 10f; // 물체가 떨어질 높이

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 위치의 좌표
                Vector3 dropPosition = hit.point;
                dropPosition.y += dropHeight; // 지정한 높이에서 떨어지도록 설정

                // 물체 생성
                Instantiate(objectToDrop, dropPosition, Quaternion.identity);
            }
        }
    }
}
