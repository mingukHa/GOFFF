using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class DynamicNavMeshBaker : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 몬스터나 NavMesh를 따라다닐 대상
    [SerializeField]
    private float updateInterval = 1f; // NavMesh를 갱신하는 간격 (초)
    [SerializeField]
    private float bakeRadius = 10f; // NavMesh 재생성 반경

    private NavMeshSurface navMeshSurface;
    private float timer = 0f;

    private void Start()
    {
        // NavMeshSurface 컴포넌트 가져오기
        navMeshSurface = GetComponent<NavMeshSurface>();

        if (target == null)
        {
            Debug.LogError("타겟이 설정되지 않았습니다. 타겟을 설정하세요.");
        }

        // 최초 NavMesh Bake
        BakeNavMeshAroundTarget();
    }

    private void Update()
    {
        if (target == null) return;

        // 일정 시간 간격으로 NavMesh 업데이트
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            BakeNavMeshAroundTarget();
            timer = 0f;
        }
    }

    private void BakeNavMeshAroundTarget()
    {
        // NavMeshSurface 위치를 타겟(몬스터)로 이동
        navMeshSurface.transform.position = target.position;

        // NavMeshSurface의 크기를 Bake 반경에 맞게 조정
        navMeshSurface.transform.localScale = new Vector3(bakeRadius, 1, bakeRadius);

        // NavMesh 재생성
        navMeshSurface.BuildNavMesh();
        Debug.Log($"NavMesh가 {target.position} 주변으로 Bake되었습니다.");
    }
}