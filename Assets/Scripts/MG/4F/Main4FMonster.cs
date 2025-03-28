using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MainF4onster : MonoBehaviourPun //4층 몬스터 메인 FSM
{
    [SerializeField] private Vector3 Cubedetect = new Vector3 (10, 10, 10);
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float visionDistance = 5f;
    [SerializeField] private float idleTimeLimit = 10f;
    [SerializeField] private float arrivalThreshold = 2f;
    [SerializeField] private NavMeshAgent navAgent;
    //private PlayerDead pd;
    private Animator animator;

    private enum MonsterState { Idle, Walking, LookingAround, Attack,  Detect }
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Transform detectedTarget;
    private float questTime;
    private float detectTime;

    private bool isDetectedByZombie = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
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
                HandleDetectState();
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
    public void Walk()
    {
        SoundManager.instance.SFXPlay("ZomBreathing_SFX", this.gameObject);
    }
    public void Idlesound()
    {
        SoundManager.instance.SFXPlay("ZomBreathing_SFX", this.gameObject);
    }
    private void HandleIdleState()
    {
        DetectTargetsInView();
    }
    
    public void Detect()
    {
        //SoundManager.instance.SFXPlay("ZomShout_SFX",this.gameObject);

        if (!isDetectedByZombie)  // 아직 발각되지 않았다면
        {
            isDetectedByZombie = true;  // 발각 상태로 변경
            photonView.RPC("OnZombieDetected", RpcTarget.All);  // 모든 클라이언트에게 발각된 상태 전달
        }
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
    //public void Walk()
    //{
    //   // SoundManager.instance.SFXPlay("ZomWalk_SFX");
    //}
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
        SetTargetPosition(collisionPoint);
        currentState = MonsterState.Walking;
    }
    private void HandlePlayerCollision(Vector3 collisionPoints)
    {
        SetTargetPosition(collisionPoints);
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
                   // SoundManager.instance.photonView.RPC("OnZombieDetected", RpcTarget.All);
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
          Vector3 targetPosition = detectedTarget.position + Vector3.left*2f;

          // NavMesh 위 좌표 보정
          NavMeshHit hit;
          if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
          {
              // 보정된 위치로 순간이동
              transform.position = hit.position;
              SoundManager.instance.SFXPlay("GameOver_SFX", this.gameObject);
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
        Gizmos.DrawWireCube(transform.position, Cubedetect);

        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

}
