using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    [SerializeField] private DomwaiterOven triggerZone0;   // 주황색 박스 [0]
    [SerializeField] private Transform targetPosition;  // 주황색 박스 [1]

    public void OnButtonPressed()
    {
        // 주황색 박스[0]dptj 충돌된 모든 오브젝트 가져오기
        var objectsToMove = triggerZone0.GetObjectsInZone();

        // 모든 오브젝트를 주황색 박스[1] 위치로 이동
        foreach (GameObject obj in objectsToMove)
        {
            obj.transform.position = targetPosition.position;
        }
    }
}