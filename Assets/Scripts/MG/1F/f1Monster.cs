using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class F1Monster : MonoBehaviour //1�� ����
{
    private NavMeshAgent navMeshAgent; // NavMeshAgent ������Ʈ
    public float detectionRadius = 40f; // �÷��̾ Ž���� ������
    public LayerMask isTarget; // Ž���� ��� ���̾� (�÷��̾� �±�)
    private Animator animator; //���ϸ����� �޾ƿ���
    private Transform target; // �÷��̾��� Transform

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent �ʱ�ȭ
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePath()); // Ž�� �� �̵� �ڷ�ƾ ����
    }

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            // ���� ������ "Player" �±׸� ���� ��ü Ž��
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, isTarget);
            if (colliders.Length > 0)
            {
                // ���� ����� �÷��̾ Ÿ������ ����
                target = colliders[0].transform;
            }
            else
            {
                target = null; // Ÿ�� ������ �ʱ�ȭ
            }

            // NavMeshAgent�� Ÿ���� ����
            if (target != null)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("isWalking", true);
            }
            else
            {
                navMeshAgent.isStopped = true; // Ÿ�� ������ ����
            }

            yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� �ݺ�
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Ž�� �ݰ� �ð�ȭ�� ���� ����׿�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
