using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class F1Monster : MonoBehaviour //1층 몬스터
{
    private NavMeshAgent navMeshAgent; // NavMeshAgent 컴포넌트
    public float detectionRadius = 40f; // 플레이어를 탐지할 반지름
    public LayerMask isTarget; // 탐지할 대상 레이어 (플레이어 태그)
    private Animator animator; //에니메이터 받아오기
    private Transform target; // 플레이어의 Transform

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent 초기화
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(UpdatePath()); // 탐지 및 이동 코루틴 시작
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
