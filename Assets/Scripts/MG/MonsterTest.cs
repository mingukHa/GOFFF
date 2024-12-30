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
    private float arrivalThreshold = 0.5f; // ��ǥ ���� �������� ������ �Ÿ�
    [SerializeField]
    private NavMeshAgent navAgent; // NavMeshAgent
    private Animator animator; // ������ �ִϸ�����

    private enum MonsterState { Idle, Walking, Quest, Attack, Returning, Detect } // ������ ����
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // ���� ���� ��ġ
    private Vector3 targetPosition = Vector3.zero; // �̵��� ��ǥ ����
    private Transform detectedTarget; // Ž���� Ÿ��
    private float questTime; // Ž�� ���� �ð�

    private void Awake()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
    }

    private void Start()
    {
        originalPosition = transform.position; // ������ ���� ��ġ ����
        GroundCollision.OnObjectHitGround += HandleGroundCollision; // �浹 �̺�Ʈ ����
    }

    private void Update()
    {
        DetectTargetsInView(); // �þ߰��� �Ÿ� ��� Ž��

        Debug.Log($"���� ����: {currentState}, TargetPosition: {targetPosition}");

        switch (currentState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Walking:
                MoveToTarget();
                break;
            case MonsterState.Quest:
                LookAround(); // �ֺ��� Ž��
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

    // �浹 �̺�Ʈ ó��
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
        Debug.Log($"�ݹ鿡�� ���޵� �浹 ��ǥ: {collisionPoint}");

        // Z �� ����
        collisionPoint.z = collisionPoint.z == 0 ? transform.position.z : collisionPoint.z;

        // �浹 �������� �ڷ� ������ ��ġ�� ����
        Vector3 offset = (transform.position - collisionPoint).normalized * arrivalThreshold;
        collisionPoint += offset;

        SetTargetPosition(collisionPoint); // ��ǥ ���� ����
        currentState = MonsterState.Walking; // ���¸� Walking���� ��ȯ
        Debug.Log($"�浹 ��ǥ�� �̵�: {targetPosition}");
    }

    // ��ǥ ��ġ ����
    private void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition; // ��ǥ ��ġ ����
        Debug.Log($"TargetPosition ������: {targetPosition}");
    }

    // �ֺ� �þ� �� Ÿ�� Ž��
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
                    SetTargetPosition(collider.transform.position);
                    currentState = MonsterState.Walking;
                    return;
                }
            }
        }

        detectedTarget = null;
    }

    // ��ǥ �������� �̵�
    private void MoveToTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position; // NavMesh ��ǥ�� ����
            }

            navAgent.SetDestination(targetPosition); // ��ǥ �������� �̵�
            animator.SetTrigger("isWalking"); // �ȱ� �ִϸ��̼� ����
            Debug.Log($"��ǥ �������� �̵� ��: {targetPosition}");

            // ��ǥ ������ ���� �Ÿ� ���� �����ϸ� Ž�� ���·� ��ȯ
            if (Vector3.Distance(transform.position, targetPosition) <= arrivalThreshold && !navAgent.pathPending)
            {
                navAgent.ResetPath(); // �̵� ����
                currentState = MonsterState.Quest; // Ž�� ���·� ��ȯ
                questTime = 0f; // Ž�� �ð� �ʱ�ȭ
                Debug.Log("��ǥ ������ ����, Ž�� ���·� ��ȯ");
            }
        }
    }

    // �ֺ� Ž��
    private void LookAround()
    {
        questTime += Time.deltaTime;

        if (questTime < 3f) // 3�� ���� �ֺ� Ž��
        {
            transform.Rotate(Vector3.up, 180f * Time.deltaTime); // ȸ��
            animator.SetTrigger("isLookingAround"); // Ž�� �ִϸ��̼� ����
            Debug.Log("�ֺ��� Ž�� ��...");
        }
        else
        {
            currentState = MonsterState.Idle; // Ž�� �� ��� ���·� ��ȯ
            Debug.Log("Ž�� �Ϸ�, ��� ���·� ��ȯ");
        }
    }

    // Ÿ���� ����
    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            navAgent.SetDestination(detectedTarget.position); // Ÿ������ �̵�
            if (Vector3.Distance(transform.position, detectedTarget.position) <= navAgent.stoppingDistance)
            {
                animator.SetTrigger("AttackTrigger"); // ���� �ִϸ��̼� ����
                Debug.Log($"�÷��̾� ���� ��: {detectedTarget.position}");
            }
        }
    }

    // ���� ��ġ�� ���ư���
    private void ReturnToOriginalPosition()
    {
        navAgent.SetDestination(originalPosition); // ���� ��ġ�� �̵�
        Debug.Log("���� ��ġ�� ���ư��� ��...");
    }

    // ����׿� Gizmo �׸���
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Ž�� �ݰ� �ð�ȭ

        Vector3 forward = transform.forward * visionDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary); // ���� �þ�
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary); // ������ �þ�
    }
}
