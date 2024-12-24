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
    private enum MonsterState { Idle, Walking, Quest, Attack, Returning , Detect} //���, �ȱ� , Ž��, ����, ���ư���
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
        originalPosition = transform.position; //���� ������ ���� �صΰ�
    }

    private void Update()
    {
        // Ÿ�� ������ ��� ���¿��� ���������� ����
        DetectTargetsInView();

        switch (currentState)
        {
            case MonsterState.Idle: //�⺻ ���¿�����
                
                break;
            case MonsterState.Walking: //���� ����
                
                break;
            case MonsterState.Quest: //Ž�� �� ����
               
                break;
            case MonsterState.Attack: //���ݽ�
                
                break;
            case MonsterState.Returning: //���ư���
                
                break;
            case MonsterState.Detect: //���� Ž�� ��

                break;
        }
    }

    private void DetectTargetsInView() //�⺻ Ž�� ����
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
        //���� �ݶ��̴��� �׷��� Ž��
        foreach (Collider collider in hitColliders) //�ݶ��̴��� Ž�� �� ������ �迭�� �����´�
        {
            Vector3 directionToTarget = (collider.transform.position - transform.position).normalized; //�ݶ��̴��� �����ǿ� ���� �������� ���� �ش� ������ ���Ѵ�
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget); 

            // �þ߰� �� �Ÿ� Ȯ��
            if (angleToTarget <= viewAngle / 2 && Vector3.Distance(transform.position, collider.transform.position) <= visionDistance)
            {
                if (collider.CompareTag("Player")) //���� �� ��ü�� �÷��̾� �±׸� ������ �ִٸ�
                {
                    Debug.Log("�÷��̾� �߰�");
                    detectedTarget = collider.transform; //Ÿ�� �÷��̾��� �ݶ��̴� Ʈ�������� �����´�
                    currentState = MonsterState.Attack; // �÷��̾� �߰� �� ��� ���� ���·� ��ȯ
                    Debug.Log("�÷��̾� ����");
                    return;
                }
                else
                {
                    Debug.Log("�÷��̾� ã�� �� ��");
                    targetPosition = collider.transform.position;
                    if (currentState != MonsterState.Walking)
                        currentState = MonsterState.Walking; // ��� �Ǵ� �Ϲ� Ÿ�� ���� �� Walking���� ��ȯ                   
                    return;
                }
            }
        }

        // Ÿ���� ������ ������ Ÿ�� �ʱ�ȭ
        detectedTarget = null;
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

