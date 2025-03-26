using UnityEngine;
using UnityEngine.AI;

public class MonsterSlow : MonoBehaviour //몬스터가 느려지는 로직
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();

            agent.speed = 1f;
        }
    }
}
