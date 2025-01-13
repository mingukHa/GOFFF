using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CopyRoomWatcher : MonoBehaviour
{
    public Transform[] objects; // 검사할 오브젝트 리스트
    public Transform[] targetsTr;         // 목표 위치
    public float checkRadius = 4.0f;       // 목표 위치 허용 오차(반경)
    private int objectsAtTarget = 0;       // 목표 위치에 도달한 오브젝트 개수
    private bool elevatorOpen = false;

    public BoxCollider buttoncollider = null;

    IEnumerator watch = null;

    public bool ElevatorOpen {  get { return elevatorOpen; } }

    private void Start()
    {
        watch = WatchCoroutine();
        StartCoroutine(watch);
        buttoncollider.enabled = false;
    }

    private IEnumerator WatchCoroutine()
    {
        objectsAtTarget = 0; // 매 프레임마다 초기화

        for(int i = 0;  i < objects.Length; i++)
        {
            if (objects[i] == null) continue; // 오브젝트가 없으면 무시

            // 오브젝트와 목표 위치 간 거리 계산
            float distance = Vector3.Distance(objects[i].position, targetsTr[i].position);

            if (distance <= checkRadius)
            {
                objectsAtTarget++; // 목표 위치에 도달한 오브젝트 개수 증가
            }
        }

        // 조건 성립 확인
        if (objectsAtTarget >= 5)
        {
            Debug.Log("5개의 오브젝트가 목표 위치에 도달했습니다!");
            // 조건 성립 시 추가 동작 수행
            StopCoroutine(watch);
            buttoncollider.enabled = true;
        }

        yield return new WaitForSeconds(1f);
    }
}
