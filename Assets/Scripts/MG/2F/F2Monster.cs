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
    private NavMeshAgent navMeshAgent; // NavMeshAgent 컴포넌트
    public float detectionRadius = 10f; // 플레이어를 탐지할 반지름
    public LayerMask isTarget; // 탐지할 대상 레이어 (플레이어 태그)
    private Animator animator;
    private Transform target; // 플레이어의 Transform
    private GameObject detectedTarget;

    private bool triggers = true;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent 초기화
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
                Debug.Log("좀비 버거 받음");
                animator.SetBool("isWalk", true);
                SetKey();

                photonView.RPC("SetKey", RpcTarget.All);
            }
            else if ((other.gameObject.CompareTag("Grabbable")))
            {
                Debug.Log("좀비 딴거 받음");
                animator.SetBool("isAttack", true);
                AttackTarget();
            }
      //  }
    }
    [PunRPC]
    private void SetKey()
    {
        Debug.Log("setkey실행 됨");
        Key.gameObject.SetActive(true);
        SoundManager.instance.SFXPlay("DropKey_SFX", this.gameObject);
        //키 떨구는 소리 추가
        Bugger.gameObject.SetActive(false);
        propertyBlock = new MaterialPropertyBlock();
        thia.enabled = false;
        // 아웃라인 색을 바뀌게 하는 코드
        skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_OutlineColor", Color.white);
        skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        
        navMeshAgent.SetDestination(target.position);
        Debug.Log($"{target.position}으로 이동 중");
        StopCoroutine(Sounds());
    }
    private void AttackTarget()
    {
        StopCoroutine(Sounds());
        StartCoroutine(UpdatePath()); // 탐지 및 이동 코루틴 시작
        propertyBlock = new MaterialPropertyBlock();

        // 아웃라인 색을 바뀌게 하는 코드
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_OutlineColor", Color.red);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        
            // 공격 애니메이션 재생
            animator.SetTrigger("isAttack");

            // 타겟 초기화 및 상태 전환
            detectedTarget = null; // 타겟 초기화
            
        }
    private IEnumerator UpdatePath()
    {
        while (true)
        {
            // 범위 내에서 "Player" 태그를 가진 객체 탐지
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, isTarget);
            if (colliders.Length > 0)
            {
                // 가장 가까운 플레이어를 타겟으로 설정
                target = colliders[0].transform;
            }
            else
            {
                target = null; // 타겟 없으면 초기화
            }

            // NavMeshAgent로 타겟을 따라감
            if (target != null)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.position);
                animator.SetBool("isWalking", true);
            }
            else
            {
                navMeshAgent.isStopped = true; // 타겟 없으면 멈춤
            }

            yield return new WaitForSeconds(0.5f); // 0.5초마다 반복
        }
    }






    private void OnDrawGizmosSelected()
    {
        // 탐지 반경 시각화를 위한 디버그용
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
