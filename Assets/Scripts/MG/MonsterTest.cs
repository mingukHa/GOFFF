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
    private float arrivalThreshold = 1.5f; // ��ǥ ���� �������� ������ �Ÿ�
    [SerializeField]
    private NavMeshAgent navAgent; // NavMeshAgent
    private Animator animator; // ������ �ִϸ�����

    private enum MonsterState { Idle, Walking, LookingAround, Attack, Returning, Detect } // ������ ����
    private MonsterState currentState = MonsterState.Idle;

    private Vector3 originalPosition; // ���� ���� ��ġ
    private Vector3 targetPosition = Vector3.zero; // �̵��� ��ǥ ����
    private Transform detectedTarget; // Ž���� Ÿ��
    private float questTime; // Ž�� ���� �ð�
    private float detectTime = 0f;

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

        switch (currentState)
        {
            case MonsterState.Idle:
                break;
            case MonsterState.Walking:
                MoveToTarget(); // ��ǥ �������� �̵�
                break;
            case MonsterState.LookingAround:
                LookAround(); // �ֺ� Ž��
                break;
            //case MonsterState.Returning:
            //    ReturnToOriginalPosition(); // ���� ��ġ�� ����
            //    break;
            case MonsterState.Detect:
                HandleDetectState();
                break;
            case MonsterState.Attack:
                Debug.Log("attack");
                AttackTarget(); // Ÿ�� ����
                break;
        }
    }
    private void HandleDetectState()
    {
        detectTime += Time.deltaTime; // Detect ���¿��� ��� �ð� ����

        if (detectTime >= 2f) // Detect ���¿��� 2�� ��� ��
        {
            currentState = MonsterState.Attack; // Attack ���·� ��ȯ
            animator.SetTrigger("isAttack");
            detectTime = 0f; // Ÿ�̸� �ʱ�ȭ
            animator.SetBool("isDetecting", false); // Detect �ִϸ��̼� ����
            Debug.Log("Detect ���¿��� 2�� ��� �� Attack ���·� ��ȯ");
        }
    }

    // �浹 �̺�Ʈ ó��
    private void HandleGroundCollision(Vector3 collisionPoint)
    {
      //  Debug.Log($"�ݹ鿡�� ���޵� �浹 ��ǥ: {collisionPoint}");

        // Z �� ����
        collisionPoint.z = collisionPoint.z == 0 ? transform.position.z : collisionPoint.z;

        // �浹 �������� �ڷ� ������ ��ġ�� ����
        Vector3 offset = (transform.position - collisionPoint).normalized * arrivalThreshold;
        collisionPoint += offset;

        SetTargetPosition(collisionPoint); // ��ǥ ���� ����
        currentState = MonsterState.Walking; // ���¸� Walking���� ��ȯ
      //  Debug.Log($"�浹 ��ǥ�� �̵�: {targetPosition}");
    }

    // ��ǥ ��ġ ����
    private void SetTargetPosition(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition; // ��ǥ ��ġ ����
     //   Debug.Log($"TargetPosition ������: {targetPosition}");
    }

    private void DetectTargetsInView()
    {
        // Ž�� �ݰ� ���� �ݶ��̴� �˻�
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        foreach (Collider collider in hitColliders)
        {
            Vector3 directionToTarget = (collider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // �þ߰��� �Ÿ� ���� �ִ� ���
            if (angleToTarget <= viewAngle / 2 && Vector3.Distance(transform.position, collider.transform.position) <= visionDistance)
            {
                if (collider.CompareTag("Player"))
                {
                    // �÷��̾ �߰��� ���
                    detectedTarget = collider.transform;

                    // Detect �ִϸ��̼� �� ����
                    animator.SetBool("isDetecting", true);
                    currentState = MonsterState.Detect;

                    Debug.Log("�÷��̾ Ž���߽��ϴ�!");
                    return; // �߰� ��� ����
                }
            }
        }

        // �÷��̾ Ž������ ���� ���
        animator.SetBool("isDetecting", false);
        detectedTarget = null;
    }


    private void MoveToTarget()
    {
        if (targetPosition != Vector3.zero)
        {
            // NavMesh ���� �ִ��� Ȯ�� �� ����
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position; // NavMesh ��ǥ�� ����
            }

            navAgent.SetDestination(targetPosition); // ��ǥ �������� �̵�
            animator.SetBool("isWalking", true); // �ȱ� �ִϸ��̼� ����

            // ���� ����
            if (!navAgent.pathPending && navAgent.remainingDistance <= arrivalThreshold)
            {
                navAgent.ResetPath(); // �̵� ����
                animator.SetBool("isWalking", false); // �ȱ� �ִϸ��̼� ����
                currentState = MonsterState.LookingAround; // Ž�� ���·� ��ȯ
                questTime = 0f; // Ž�� �ð� �ʱ�ȭ
                Debug.Log("��ǥ ������ ����, Ž�� ���·� ��ȯ");
            }
        }
    }

    private void LookAround()
    {
        questTime += Time.deltaTime; // Ž�� �ð� ����

        if (questTime < 3f) // 3�� ���� Ž��
        {
            transform.Rotate(Vector3.up, 40f * Time.deltaTime); // ���� ȸ��
            animator.SetBool("isLookingAround", true); // Ž�� �ִϸ��̼� ����
            Debug.Log("�ֺ��� Ž�� ��...");
        }
        else
        {
            animator.SetBool("isLookingAround", false); // Ž�� �ִϸ��̼� ����
            currentState = MonsterState.Idle; // ��� ���·� ��ȯ
            Debug.Log("Ž�� �Ϸ�, ��� ���·� ��ȯ");
        }
    }


    // Ÿ���� ����
    private void AttackTarget()
    {
        if (detectedTarget != null)
        {
            // �÷��̾� ��ġ ��������
            Vector3 targetPosition = detectedTarget.position;
            Debug.Log($"��ǥ ��ġ :{detectedTarget.position}");
            // �����̵� ��ǥ ���� (�÷��̾� ��ġ)
            Vector3 teleportPosition = targetPosition + Vector3.up * 0.5f; // �ణ ���� �̵�
            transform.position = teleportPosition;

          
            // ���¸� Attack ���� (�ʿ�� �ٸ� ���·� ��ȯ ����)
            currentState = MonsterState.Attack;
        }
    }
    private void OnCollisionEnter(Collision collision) //�÷��̾� ��� ó��
    {
        // �浹�� ��ü�� Player �±׸� ���� ���
        if (collision.collider.CompareTag("Player"))
        {
            // PlayerHealth ������Ʈ�� ������ ��� ó��
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                //player.Die(); // �÷��̾� ��� ó��
                Debug.Log("�÷��̾ óġ�߽��ϴ�!");
            }
        }
    }




    //// ���� ��ġ�� ���ư���
    //private void ReturnToOriginalPosition()
    //{
    //    navAgent.SetDestination(originalPosition); // ���� ��ġ�� �̵�
    //  //  Debug.Log("���� ��ġ�� ���ư��� ��...");
    //}

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
