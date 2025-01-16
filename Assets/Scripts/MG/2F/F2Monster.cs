using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Photon.Pun;

public class F2Monster : MonoBehaviourPun
{
    [SerializeField] private GameObject Key;
    [SerializeField] private GameObject TargetPoint;
    [SerializeField] private GameObject Bugger;
    private Material mr;
    private NavMeshAgent navMeshAgent; // NavMeshAgent ������Ʈ
    public float detectionRadius = 10f; // �÷��̾ Ž���� ������
    public LayerMask isTarget; // Ž���� ��� ���̾� (�÷��̾� �±�)
    private Animator animator;
    private Transform target; // �÷��̾��� Transform
    private GameObject detectedTarget;

    private bool triggers = true;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent �ʱ�ȭ
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
    }
    private void Start()
    {
        target = TargetPoint.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (triggers == true)
       // {
            triggers = false;
            if (other.gameObject.CompareTag("Bugger"))
            {
                Debug.Log("���� ���� ����");
                animator.SetBool("isWalk", true);
                SetKey();
                photonView.RPC("SetKey", RpcTarget.All);
            }
            else if ((other.gameObject.CompareTag("Grabbable")))
            {
                Debug.Log("���� ���� ����");
                animator.SetBool("isAttack", true);
                AttackTarget();
            }
      //  }
    }
    [PunRPC]
    private void SetKey()
    {
        Debug.Log("setkey���� ��");
        Key.gameObject.SetActive(true);
        //Ű ������ �Ҹ� �߰�
        Bugger.gameObject.SetActive(false);
        propertyBlock = new MaterialPropertyBlock();

        // �ƿ����� ���� �ٲ�� �ϴ� �ڵ�
        skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_OutlineColor", Color.white);
        skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        
        navMeshAgent.SetDestination(target.position);
        Debug.Log($"{target.position}���� �̵� ��");
    }
    private void AttackTarget()
    {
        propertyBlock = new MaterialPropertyBlock();

        // �ƿ����� ���� �ٲ�� �ϴ� �ڵ�
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_OutlineColor", Color.red);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, isTarget);
            if (colliders.Length > 0)
            {       
                detectedTarget = colliders[0].gameObject;
            }
        if (detectedTarget != null)
        {
            navMeshAgent.enabled = false;

            // �÷��̾� ��ġ ��������
            Vector3 targetPosition = detectedTarget.transform.position + Vector3.forward * 2f;


            // NavMesh �� ��ǥ ����
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 1.0f, NavMesh.AllAreas))
            {
                // ������ ��ġ�� �����̵�
                transform.position = hit.position;
                Debug.Log($"�ڷ���Ʈ �Ϸ�: {hit.position}");
            }
            else
            {
                Debug.LogWarning("�÷��̾� ��ġ�� NavMesh ���� �����ϴ�!");
            }

            navMeshAgent.enabled = true;

            // ���� �ִϸ��̼� ���
            animator.SetTrigger("isAttack");

            // Ÿ�� �ʱ�ȭ �� ���� ��ȯ
            detectedTarget = null; // Ÿ�� �ʱ�ȭ
            
        }
        else
        {
            Debug.LogWarning("Ÿ���� �����ϴ�!");
            
        }
    }

    //private void AttackMonster()
    //{
    //    StartCoroutine(UpdatePath()); // Ž�� �� �̵� �ڷ�ƾ ����
    //}

    //private IEnumerator UpdatePath()
    //{
    //    while (true)
    //    {
    //        // ���� ������ "Player" �±׸� ���� ��ü Ž��
    //        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, isTarget);
    //        if (colliders.Length > 0)
    //        {
    //            // ���� ����� �÷��̾ Ÿ������ ����
    //            target = colliders[0].transform;
    //        }
    //        else
    //        {
    //            target = null; // Ÿ�� ������ �ʱ�ȭ
    //        }

    //        // NavMeshAgent�� Ÿ���� ����
    //        if (target != null)
    //        {
    //            navMeshAgent.isStopped = false;
    //            navMeshAgent.SetDestination(target.position);
    //            animator.SetBool("isWalking", true);
    //        }
    //        else
    //        {
    //            navMeshAgent.isStopped = true; // Ÿ�� ������ ����
    //        }

    //        yield return new WaitForSeconds(0.5f); // 0.5�ʸ��� �ݺ�
    //    }
    //}

    private void OnDrawGizmosSelected()
    {
        // Ž�� �ݰ� �ð�ȭ�� ���� ����׿�
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
