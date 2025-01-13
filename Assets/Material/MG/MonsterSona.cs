using UnityEngine;
using System.Collections;

public class MonsterSona : MonoBehaviour
{
    private SimpleSonarShader_Parent par; // Inspector에서 직접 설정
    private Animator animator;
    //private bool monster = true;
    private enum MonsterState { Idle, Walking, LookingAround, Attack, Returning, Detect }
    private MonsterState currentState = MonsterState.Idle;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // 코루틴 시작
        par = FindFirstObjectByType<SimpleSonarShader_Parent>();
        

    }
    private void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                StartCoroutine(MonstersonaIdle(1,1));
                break;
            case MonsterState.Walking:
                StartCoroutine(MonstersonaIdle(0.5f,2));
                break;
            case MonsterState.LookingAround:
                
                break;
            case MonsterState.Detect:

                StartCoroutine(MonstersonaIdle(0.5f, 3));
                break;
            case MonsterState.Attack:
                
                break;
        }
    }
    private IEnumerator MonstersonaIdle(float time, int powar)
    {
        Debug.Log("코루틴 실행");
        while (true)
        {
            Vector4 position = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
            if (par) par.StartSonarRing(position, time, powar);
            Debug.Log("Sonar Ring 실행: " + transform.position);

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
   
}
