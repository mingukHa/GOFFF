using UnityEngine;
using UnityEngine.AI;

public class MonsterSlow : MonoBehaviour //���Ͱ� �������� ����
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
