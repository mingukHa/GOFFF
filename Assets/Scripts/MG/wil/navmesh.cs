using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class DynamicNavMeshBaker : MonoBehaviour
{
    [SerializeField]
    private Transform target; // ���ͳ� NavMesh�� ����ٴ� ���
    [SerializeField]
    private float updateInterval = 1f; // NavMesh�� �����ϴ� ���� (��)
    [SerializeField]
    private float bakeRadius = 10f; // NavMesh ����� �ݰ�

    private NavMeshSurface navMeshSurface;
    private float timer = 0f;

    private void Start()
    {
        // NavMeshSurface ������Ʈ ��������
        navMeshSurface = GetComponent<NavMeshSurface>();

        if (target == null)
        {
            Debug.LogError("Ÿ���� �������� �ʾҽ��ϴ�. Ÿ���� �����ϼ���.");
        }

        // ���� NavMesh Bake
        BakeNavMeshAroundTarget();
    }

    private void Update()
    {
        if (target == null) return;

        // ���� �ð� �������� NavMesh ������Ʈ
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            BakeNavMeshAroundTarget();
            timer = 0f;
        }
    }

    private void BakeNavMeshAroundTarget()
    {
        // NavMeshSurface ��ġ�� Ÿ��(����)�� �̵�
        navMeshSurface.transform.position = target.position;

        // NavMeshSurface�� ũ�⸦ Bake �ݰ濡 �°� ����
        navMeshSurface.transform.localScale = new Vector3(bakeRadius, 1, bakeRadius);

        // NavMesh �����
        navMeshSurface.BuildNavMesh();
        Debug.Log($"NavMesh�� {target.position} �ֺ����� Bake�Ǿ����ϴ�.");
    }
}