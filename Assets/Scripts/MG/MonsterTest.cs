using UnityEngine;
using UnityEngine.AI;

public class MonsterTest : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float visionDistance = 5f;
    [SerializeField] private float idleTimeLimit = 10f;
    [SerializeField] private float arrivalThreshold = 1.5f;
    [SerializeField] private NavMeshAgent navAgent;
    private PlayerDead pd;
    private Animator animator;

    private enum MonsterState { Idle, Walking, LookingAround, Attack, Returning, Detect }
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Transform detectedTarget;
    private float questTime;
    private float detectTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        GroundCollision.OnObjectHitGround += HandleGroundCollision;
        SimpleSonarShader_PlayerMove.PlayerMove += HandlePlayerCollision;
    }

    private void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                HandleIdleState();
                break;
            case MonsterState.Walking:
                MoveToTarget();                
                break;
            case MonsterState.LookingAround:
                LookAround();
                break;
            case MonsterState.Detect:
                HandleDetectState();
                
                break;
            case MonsterState.Attack:
                AttackTarget();
                
                break;
        }
    }
    private void Idel()
    {
        SoundManager.instance.SFXPlay("ZomBreathing_SFX");
    }
    private void HandleIdleState()
    {
        DetectTargetsInView();
    }
    public void Attack()
    {
        SoundManager.instance.SFXPlay("GameOver_SFX");
    }
    public void Detect()
    {
        SoundManager.instance.SFXPlay("ZomShout_SFX");
    }
    private void HandleDetectState()
    {
        detectTime += Time.deltaTime;

        if (detectTime >= 2f)
        {
            currentState = MonsterState.Attack;
            animator.SetTrigger("isAttack");
            detectTime = 0f;
            animator.SetBool("isDetecting", false);
        }
    }
    public void Walk()
    {
        SoundManager.instance.SFXPlay("ZomWalk_SFX");
    }
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
        SetTargetPosition(collisionPoint);
        currentState = MonsterState.Walking;
    }
    private void HandlePlayerCollision(Vector3 collisionPoints)
    {
        SetTargetPosition(collisionPoints);
        Debug.Log($"{collisionPoints}전달 받은 좌표");
        currentState = MonsterState.Walking;
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
                    detectedTarget = collider.transform;
                    animator.SetBool("isDetecting", true);
                    currentState = MonsterState.Detect;
                    return;
                }
            }
        }

        animator.SetBool("isDetecting", false);
        detectedTarget = null;
    }

    private void MoveToTarget()
    {
        //SoundManager.instance.SFXPlay("Wheel2_SFX");
        if (targetPosition != Vector3.zero)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
            }

            navAgent.SetDestination(targetPosition);
            animator.SetBool("isWalking", true);

            if (!navAgent.pathPending && navAgent.remainingDistance <= arrivalThreshold)
            {
                navAgent.ResetPath();
                animator.SetBool("isWalking", false);
                currentState = MonsterState.LookingAround;
                questTime = 0f;
            }
        }
    }

    private void LookAround()
    {
        questTime += Time.deltaTime;

        if (questTime < 3f)
        {
            transform.Rotate(Vector3.up, 40f * Time.deltaTime);
            animator.SetBool("isLookingAround", true);
        }
        else
        {
            animator.SetBool("isLookingAround", false);
            currentState = MonsterState.Idle;
        }
    }

    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            navAgent.enabled = false;

            // 플레이어 위치 가져오기
            Vector3 targetPosition = detectedTarget.position + Vector3.forward*2f;

            // NavMesh 위 좌표 보정
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                // 보정된 위치로 순간이동
                transform.position = hit.position;
                Debug.Log($"텔레포트 완료: {hit.position}");
            }
            else
            {
                Debug.LogWarning("플레이어 위치가 NavMesh 위에 없습니다!");
            }

            navAgent.enabled = true;

            // 공격 애니메이션 재생
            animator.SetTrigger("isAttack");

            // 타겟 초기화 및 상태 전환
            detectedTarget = null; // 타겟 초기화
            currentState = MonsterState.Idle; // Idle 상태로 복귀
        }
        else
        {
            Debug.LogWarning("타겟이 없습니다!");
            currentState = MonsterState.Idle; // Idle 상태로 복귀
        }
    }


    private void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}
