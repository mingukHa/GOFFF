using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Navmesh : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 몬스터나 NavMesh를 따라다닐 대상
    [SerializeField]
    private float updateInterval = 1f; // NavMesh를 갱신하는 간격
    [SerializeField]
    private float bakeRadius = 10f; // NavMesh 재생성 반경

    private NavMeshSurface navMeshSurface; //네브메쉬 굽는 애 받아 오기
    private float timer = 0f;

    private void Start()
    {
        // NavMeshSurface 컴포넌트 가져오기
        navMeshSurface = GetComponent<NavMeshSurface>(); //컴포넌트 정보 받아오기

        if (target == null) //타겟 오류 설정
        {
            Debug.LogError("타겟이 설정되지 않았습니다. 타겟을 설정하세요.");
        }

        // 최초 NavMesh Bake
        BakeNavMeshAroundTarget(); //첫 시작으로 한번 굽기
    }

    private void Update()
    {
        if (target == null) return; //타겟이 없다면 돌아가기

        // 일정 시간 간격으로 NavMesh 업데이트
        timer += Time.deltaTime; 
        if (timer >= updateInterval)
        {
            BakeNavMeshAroundTarget();
            timer = 0f;//한번 굽고 타이머 초기화
        }
    }

    private void BakeNavMeshAroundTarget()
    {
        // 위치로 타겟 이동
        navMeshSurface.transform.position = target.position;

        // 베이크 스케일 조정
        navMeshSurface.transform.localScale = new Vector3(bakeRadius, 1, bakeRadius);

        // NavMesh 재생성
        navMeshSurface.BuildNavMesh();
    }
}