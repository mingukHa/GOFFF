using UnityEngine;
using System.Collections;

public class MonsterSona : MonoBehaviour
{
    [SerializeField]
    private SimpleSonarShader_Parent par; // Inspector에서 직접 설정
    //private bool monster = true;

    private void Start()
    {
        // 코루틴 시작
        StartCoroutine(Monstersona());
    }

    private IEnumerator Monstersona()
    {
        Debug.Log("코루틴 실행");
        while (true)
        {
            Vector4 position = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
            if (par) par.StartSonarRing(position, 1.0f, 1);
            Debug.Log("Sonar Ring 실행: " + transform.position);

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
}




//using UnityEngine;
//using System.Collections;

//public class MonsterSona : MonoBehaviour
//{
//    private SimpleSonarShader_Parent par; // Inspector에서 직접 설정
//    private Animator animator;
//    //private bool monster = true;
//    private enum MonsterState { Idle, Walking, Detect }
//    private MonsterState currentState = MonsterState.Idle;
//    private void Awake()
//    {
//        animator = GetComponent<Animator>();
//    }
//    private void Start()
//    {
//        // 코루틴 시작
//        par = FindFirstObjectByType<SimpleSonarShader_Parent>();
//        //StartCoroutine("MonstersonaIdle");
//    }

//    private IEnumerator MonstersonaIdle()
//    {
//        Debug.Log("코루틴 실행");

//        switch (currentState)
//        {
//            case MonsterState.Idle:
//                MonsterSonas(1.4f, 1);
//                yield return new WaitForSeconds(2f);
//                break;
//            case MonsterState.Walking:
//                MonsterSonas(1f, 2);
//                yield return new WaitForSeconds(1.3f);
//                break;
//            case MonsterState.Detect:
//                MonsterSonas(0.7f, 2);
//                yield return new WaitForSeconds(0.7f);
//                break;

//        }
//    }

//   private void MonsterSonas(float intensity, int type)
//    {
//        while (true)
//        {
//            Vector4 position = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
//            if (par) par.StartSonarRing(position, intensity, type);       
//        }
//    }
//}
