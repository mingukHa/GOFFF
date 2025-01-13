using UnityEngine;
using System.Collections;

public class MonsterSona : MonoBehaviour
{
    private SimpleSonarShader_Parent par; // Inspector���� ���� ����
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
        // �ڷ�ƾ ����
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
        Debug.Log("�ڷ�ƾ ����");
        while (true)
        {
            Vector4 position = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
            if (par) par.StartSonarRing(position, time, powar);
            Debug.Log("Sonar Ring ����: " + transform.position);

            // 1�� ���
            yield return new WaitForSeconds(1f);
        }
    }
   
}
