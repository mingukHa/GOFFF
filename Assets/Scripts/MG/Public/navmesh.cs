using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class Navmesh : MonoBehaviour
{
    [SerializeField]
    private Transform target; // ���ͳ� NavMesh�� ����ٴ� ���
    [SerializeField]
    private float updateInterval = 1f; // NavMesh�� �����ϴ� ����
    [SerializeField]
    private float bakeRadius = 10f; // NavMesh ����� �ݰ�

    private NavMeshSurface navMeshSurface; //�׺�޽� ���� �� �޾� ����
    private float timer = 0f;

    private void Start()
    {
        // NavMeshSurface ������Ʈ ��������
        navMeshSurface = GetComponent<NavMeshSurface>(); //������Ʈ ���� �޾ƿ���

        if (target == null) //Ÿ�� ���� ����
        {
            Debug.LogError("Ÿ���� �������� �ʾҽ��ϴ�. Ÿ���� �����ϼ���.");
        }

        // ���� NavMesh Bake
        BakeNavMeshAroundTarget(); //ù �������� �ѹ� ����
    }

    private void Update()
    {
        if (target == null) return; //Ÿ���� ���ٸ� ���ư���

        // ���� �ð� �������� NavMesh ������Ʈ
        timer += Time.deltaTime; 
        if (timer >= updateInterval)
        {
            BakeNavMeshAroundTarget();
            timer = 0f;//�ѹ� ���� Ÿ�̸� �ʱ�ȭ
        }
    }

    private void BakeNavMeshAroundTarget()
    {
        // ��ġ�� Ÿ�� �̵�
        navMeshSurface.transform.position = target.position;

        // ����ũ ������ ����
        navMeshSurface.transform.localScale = new Vector3(bakeRadius, 1, bakeRadius);

        // NavMesh �����
        navMeshSurface.BuildNavMesh();
    }
}