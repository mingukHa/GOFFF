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
    private NavMeshAgent navAgent;
    private Animator animator;
    private enum MonsterState { Idle, Walking, Quest, Attack, Returning, Detect } // 대기, 걷기, 탐색, 공격, 돌아가기
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // 몬스터 원래 위치
    private Vector3 targetPosition; // 이동할 목표 지점
    private Transform detectedTarget; // 탐지된 타겟

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        originalPosition = transform.position; // 원래 지점을 저장
        GroundCollision.OnObjectHitGround += HandleGroundCollision;
    }

    private void Update()
    {
        DetectTargetsInView(); // 시야각과 거리 기반 탐지

        switch (currentState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Walking:
                MoveToTarget();
                break;
            case MonsterState.Quest:
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
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
        // 충돌 좌표를 목표 위치로 설정
        targetPosition = collisionPoint;
        currentState = MonsterState.Walking; // 상태를 Walking으로 변경
           
        Debug.Log($"충돌 좌표로 이동: {collisionPoint}");
    }

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
                    targetPosition = collider.transform.position;
                    currentState = MonsterState.Walking;
                    return;
                }
            }
        }

        detectedTarget = null;
    }

    private void MoveToTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            navAgent.SetDestination(targetPosition);
            animator.SetTrigger("isWalking");
            Debug.Log($"목표 지점으로 이동 중: {targetPosition}");
        }
    }

    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            navAgent.SetDestination(detectedTarget.position);
            if (Vector3.Distance(transform.position, detectedTarget.position) <= navAgent.stoppingDistance)
            {
                animator.SetTrigger("AttackTrigger");
                Debug.Log($"플레이어 공격 중: {detectedTarget.position}");
            }
        }
    }

    private void ReturnToOriginalPosition()
    {
        navAgent.SetDestination(originalPosition);
        Debug.Log("원래 위치로 돌아가는 중...");
    }

    private void OnDrawGizmosSelected()
    {
        // 탐지 반경 시각화
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // 시야각 시각화
        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}