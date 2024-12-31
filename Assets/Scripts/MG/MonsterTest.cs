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
    private float arrivalThreshold = 1.5f; // 목표 지점 도착으로 간주할 거리
    [SerializeField]
    private NavMeshAgent navAgent; // NavMeshAgent
    private Animator animator; // 몬스터의 애니메이터

    private enum MonsterState { Idle, Walking, LookingAround, Attack, Returning, Detect } // 몬스터의 상태
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // 몬스터 원래 위치
    private Vector3 targetPosition = Vector3.zero; // 이동할 목표 지점
    private Transform detectedTarget; // 탐지된 타겟
    private float questTime; // 탐색 상태 시간
    private float detectTime = 0f;

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

        switch (currentState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Walking:
                MoveToTarget(); // 목표 지점으로 이동
                break;
            case MonsterState.LookingAround:
                LookAround(); // 주변 탐색
                break;
            //case MonsterState.Returning:
            //    ReturnToOriginalPosition(); // 원래 위치로 복귀
            //    break;
            case MonsterState.Detect:
                HandleDetectState();
                break;
            case MonsterState.Attack:
                Debug.Log("attack");
                AttackTarget(); // 타겟 공격
                break;
        }
    }
    private void HandleDetectState()
    {
        detectTime += Time.deltaTime; // Detect 상태에서 경과 시간 증가

        if (detectTime >= 2f) // Detect 상태에서 2초 경과 시
        {
            currentState = MonsterState.Attack; // Attack 상태로 전환
            animator.SetTrigger("isAttack");
            detectTime = 0f; // 타이머 초기화
            animator.SetBool("isDetecting", false); // Detect 애니메이션 종료
            Debug.Log("Detect 상태에서 2초 경과 후 Attack 상태로 전환");
        }
    }

    // 충돌 이벤트 처리
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
      //  Debug.Log($"콜백에서 전달된 충돌 좌표: {collisionPoint}");

        // Z 값 보정
        collisionPoint.z = collisionPoint.z == 0 ? transform.position.z : collisionPoint.z;

        // 충돌 지점에서 뒤로 물러난 위치를 설정
        Vector3 offset = (transform.position - collisionPoint).normalized * arrivalThreshold;
        collisionPoint += offset;

        SetTargetPosition(collisionPoint); // 목표 지점 설정
        currentState = MonsterState.Walking; // 상태를 Walking으로 전환
      //  Debug.Log($"충돌 좌표로 이동: {targetPosition}");
    }

    // 목표 위치 설정
    private void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition; // 목표 위치 저장
     //   Debug.Log($"TargetPosition 설정됨: {targetPosition}");
    }

    private void DetectTargetsInView()
    {
        // 탐지 반경 내의 콜라이더 검색
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        foreach (Collider collider in hitColliders)
        {
            Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // 시야각과 거리 내에 있는 경우
            if (angleToTarget <= viewAngle / 2 && Vector3.Distance(transform.position, collider.transform.position) <= visionDistance)
            {
                if (collider.CompareTag("Player"))
                {
                    // 플레이어를 발견한 경우
                    detectedTarget = collider.transform;

                    // Detect 애니메이션 값 설정
                    animator.SetBool("isDetecting", true);
                    currentState = MonsterState.Detect;

                    Debug.Log("플레이어를 탐지했습니다!");
                    return; // 발견 즉시 종료
                }
            }
        }

        // 플레이어가 탐지되지 않은 경우
        animator.SetBool("isDetecting", false);
        detectedTarget = null;
    }


    private void MoveToTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            // NavMesh 위에 있는지 확인 및 보정
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position; // NavMesh 좌표로 보정
            }

            navAgent.SetDestination(targetPosition); // 목표 지점으로 이동
            animator.SetBool("isWalking", true); // 걷기 애니메이션 실행

            // 도착 감지
            if (!navAgent.pathPending && navAgent.remainingDistance <= arrivalThreshold)
            {
                navAgent.ResetPath(); // 이동 중지
                animator.SetBool("isWalking", false); // 걷기 애니메이션 정지
                currentState = MonsterState.LookingAround; // 탐색 상태로 전환
                questTime = 0f; // 탐색 시간 초기화
                Debug.Log("목표 지점에 도착, 탐색 상태로 전환");
            }
        }
    }

    private void LookAround()
    {
        questTime += Time.deltaTime; // 탐색 시간 증가

        if (questTime < 3f) // 3초 동안 탐색
        {
            transform.Rotate(Vector3.up, 40f * Time.deltaTime); // 몬스터 회전
            animator.SetBool("isLookingAround", true); // 탐색 애니메이션 실행
            Debug.Log("주변을 탐색 중...");
        }
        else
        {
            animator.SetBool("isLookingAround", false); // 탐색 애니메이션 종료
            currentState = MonsterState.Idle; // 대기 상태로 전환
            Debug.Log("탐색 완료, 대기 상태로 전환");
        }
    }


    // 타겟을 공격
    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            // 플레이어 위치 가져오기
            Vector3 targetPosition = detectedTarget.position;
            Debug.Log($"좌표 위치 :{detectedTarget.position}");
            // 순간이동 좌표 설정 (플레이어 위치)
            Vector3 teleportPosition = targetPosition + Vector3.up * 0.5f; // 약간 위로 이동
            transform.position = teleportPosition;

          
            // 상태를 Attack 유지 (필요시 다른 상태로 전환 가능)
            currentState = MonsterState.Attack;
        }
    }
    private void OnCollisionEnter(Collision collision) //플레이어 사망 처리
    {
        // 충돌한 객체가 Player 태그를 가진 경우
        if (collision.collider.CompareTag("Player"))
        {
            // PlayerHealth 컴포넌트를 가져와 사망 처리
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                //player.Die(); // 플레이어 사망 처리
                Debug.Log("플레이어를 처치했습니다!");
            }
        }
    }




    //// 원래 위치로 돌아가기
    //private void ReturnToOriginalPosition()
    //{
    //    navAgent.SetDestination(originalPosition); // 원래 위치로 이동
    //  //  Debug.Log("원래 위치로 돌아가는 중...");
    //}

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
