using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class MonsterTest : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 10f; // ��� Ž�� �ݰ�
    [SerializeField]
    private float viewAngle = 60f; // ���� �þ߰� (�¿� 60��)
    [SerializeField]
    private LayerMask detectionLayer; // ���� ����� ���̾�
    [SerializeField]
    private float visionDistance = 5f; // ���� �þ� �Ÿ�
    [SerializeField]
    private float idleTimeLimit = 10f; // Ž�� ���� �� ���� ��ġ�� ���� �ð�
    [SerializeField]
    private NavMeshAgent navAgent;
    private Animator animator;
    private enum MonsterState { Idle, Walking, Quest, Attack, Returning, Detect } // ���, �ȱ�, Ž��, ����, ���ư���
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // ���� ���� ��ġ
    private Vector3 targetPosition; // �̵��� ��ǥ ����
    private Transform detectedTarget; // Ž���� Ÿ��

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        originalPosition = transform.position; // ���� ������ ����
        GroundCollision.OnObjectHitGround += HandleGroundCollision;
    }

    private void Update()
    {
        DetectTargetsInView(); // �þ߰��� �Ÿ� ��� Ž��

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
        // �浹 ��ǥ�� ��ǥ ��ġ�� ����
        targetPosition = collisionPoint;
        currentState = MonsterState.Walking; // ���¸� Walking���� ����
           
        Debug.Log($"�浹 ��ǥ�� �̵�: {collisionPoint}");
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

                    Debug.Log("�÷��̾� �߰�");
                    detectedTarget = collider.transform;
                    currentState = MonsterState.Attack;
                    Debug.Log("�÷��̾� ����");
                    return;
                }
                else
                {
                    Debug.Log("�÷��̾� ã�� �� ��");
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
            Debug.Log($"��ǥ �������� �̵� ��: {targetPosition}");
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
                Debug.Log($"�÷��̾� ���� ��: {detectedTarget.position}");
            }
        }
    }

    private void ReturnToOriginalPosition()
    {
        navAgent.SetDestination(originalPosition);
        Debug.Log("���� ��ġ�� ���ư��� ��...");
    }

    private void OnDrawGizmosSelected()
    {
        // Ž�� �ݰ� �ð�ȭ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // �þ߰� �ð�ȭ
        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}