using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class MonsterTest : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 10f; // 충격 탐지 반경
    [SerializeField]
    private float viewAngle = 60f; // 근접 시야각 (좌우 60도)
    [SerializeField]
    private LayerMask detectionLayer; // 감지 대상의 레이어
    [SerializeField]
    private float visionDistance = 5f; // 근접 시야 거리
    [SerializeField]
    private float idleTimeLimit = 10f; // 탐지 없을 때 원래 위치로 복귀 시간
    [SerializeField]
    private float arrivalThreshold = 0.5f; // 목표 지점 도착으로 간주할 거리
    [SerializeField]
    private NavMeshAgent navAgent; // NavMeshAgent
    private Animator animator; // 몬스터의 애니메이터

    private enum MonsterState { Idle, Walking, Quest, Attack, Returning, Detect } // 몬스터의 상태
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // 몬스터 원래 위치
    private Vector3 targetPosition = Vector3.zero; // 이동할 목표 지점
    private Transform detectedTarget; // 탐지된 타겟
    private float questTime; // 탐색 상태 시간

    private void Awake()
    {
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
    }

    private void Start()
    {
        originalPosition = transform.position; // 몬스터의 원래 위치 저장
        GroundCollision.OnObjectHitGround += HandleGroundCollision; // 충돌 이벤트 구독
    }

    private void Update()
    {
        DetectTargetsInView(); // 시야각과 거리 기반 탐지

        Debug.Log($"현재 상태: {currentState}, TargetPosition: {targetPosition}");

        switch (currentState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Walking:
                MoveToTarget();
                break;
            case MonsterState.Quest:
                LookAround(); // 주변을 탐색
                break;
            case MonsterState.Returning:
                ReturnToOriginalPosition();
                break;
            case MonsterState.Detect:
                break;
            case MonsterState.Attack:
                AttackTarget();
                break;
        }
    }

    // 충돌 이벤트 처리
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
        Debug.Log($"콜백에서 전달된 충돌 좌표: {collisionPoint}");

        // Z 값 보정
        collisionPoint.z = collisionPoint.z == 0 ? transform.position.z : collisionPoint.z;

        // 충돌 지점에서 뒤로 물러난 위치를 설정
        Vector3 offset = (transform.position - collisionPoint).normalized * arrivalThreshold;
        collisionPoint += offset;

        SetTargetPosition(collisionPoint); // 목표 지점 설정
        currentState = MonsterState.Walking; // 상태를 Walking으로 전환
        Debug.Log($"충돌 좌표로 이동: {targetPosition}");
    }

    // 목표 위치 설정
    private void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition; // 목표 위치 저장
        Debug.Log($"TargetPosition 설정됨: {targetPosition}");
    }

    // 주변 시야 내 타겟 탐지
    private void DetectTargetsInView()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        foreach (Collider collider in hitColliders)
        {
            Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= viewAngle / 2 && Vector3.Distance(transform.position, collider.transform.position) <= visionDistance)
            {
                if (collider.CompareTag("Player"))
                {
                    Debug.Log("플레이어 발견");
                    detectedTarget = collider.transform;
                    currentState = MonsterState.Attack;
                    Debug.Log("플레이어 공격");
                    return;
                }
                else
                {
                    Debug.Log("플레이어 찾지 못 함");
                    SetTargetPosition(collider.transform.position);
                    currentState = MonsterState.Walking;
                    return;
                }
            }
        }

        detectedTarget = null;
    }

    // 목표 지점으로 이동
    private void MoveToTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position; // NavMesh 좌표로 보정
            }

            navAgent.SetDestination(targetPosition); // 목표 지점으로 이동
            animator.SetTrigger("isWalking"); // 걷기 애니메이션 실행
            Debug.Log($"목표 지점으로 이동 중: {targetPosition}");

            // 목표 지점에 일정 거리 내로 도착하면 탐색 상태로 전환
            if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold && !navAgent.pathPending)
            {
                navAgent.ResetPath(); // 이동 중지
                currentState = MonsterState.Quest; // 탐색 상태로 전환
                questTime = 0f; // 탐색 시간 초기화
                Debug.Log("목표 지점에 도착, 탐색 상태로 전환");
            }
        }
    }

    // 주변 탐색
    private void LookAround()
    {
        questTime += Time.deltaTime;

        if (questTime < 3f) // 3초 동안 주변 탐색
        {
            transform.Rotate(Vector3.up, 180f * Time.deltaTime); // 회전
            animator.SetTrigger("isLookingAround"); // 탐색 애니메이션 실행
            Debug.Log("주변을 탐색 중...");
        }
        else
        {
            currentState = MonsterState.Idle; // 탐색 후 대기 상태로 전환
            Debug.Log("탐색 완료, 대기 상태로 전환");
        }
    }

    // 타겟을 공격
    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            navAgent.SetDestination(detectedTarget.position); // 타겟으로 이동
            if (Vector3.Distance(transform.position, detectedTarget.position) <= navAgent.stoppingDistance)
            {
                animator.SetTrigger("AttackTrigger"); // 공격 애니메이션 실행
                Debug.Log($"플레이어 공격 중: {detectedTarget.position}");
            }
        }
    }

    // 원래 위치로 돌아가기
    private void ReturnToOriginalPosition()
    {
        navAgent.SetDestination(originalPosition); // 원래 위치로 이동
        Debug.Log("원래 위치로 돌아가는 중...");
    }

    // 디버그용 Gizmo 그리기
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // 탐지 반경 시각화

        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary); // 왼쪽 시야
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary); // 오른쪽 시야
    }
}
