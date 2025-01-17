using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Photon.Pun;

public class F2Monster : MonoBehaviourPun
{
    [SerializeField] private GameObject Key;
    [SerializeField] private GameObject TargetPoint;
    [SerializeField] private GameObject Bugger;
    [SerializeField] private BoxCollider thia;
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
        StartCoroutine(Sounds());
    }
    private IEnumerator Sounds()
    {
        while (true)
        {
            SoundManager.instance.SFXPlay("Hungry_SFX", this.gameObject);
            yield return new WaitForSeconds(2f);
        }
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
        SoundManager.instance.SFXPlay("DropKey_SFX", this.gameObject);
        //Ű ������ �Ҹ� �߰�
        Bugger.gameObject.SetActive(false);
        propertyBlock = new MaterialPropertyBlock();
        thia.enabled = false;
        // �ƿ����� ���� �ٲ�� �ϴ� �ڵ�
        skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_OutlineColor", Color.white);
        skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        
        navMeshAgent.SetDestination(target.position);
        Debug.Log($"{target.position}���� �̵� ��");
        StopCoroutine(Sounds());
    }
    private void AttackTarget()
    {
        StopCoroutine(Sounds());
        StartCoroutine(UpdatePath()); // Ž�� �� �̵� �ڷ�ƾ ����
        propertyBlock = new MaterialPropertyBlock();

        // �ƿ����� ���� �ٲ�� �ϴ� �ڵ�
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_OutlineColor", Color.red);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        
            // ���� �ִϸ��̼� ���
            animator.SetTrigger("isAttack");

            // Ÿ�� �ʱ�ȭ �� ���� ��ȯ
            detectedTarget = null; // Ÿ�� �ʱ�ȭ
            
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
