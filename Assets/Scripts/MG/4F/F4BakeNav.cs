using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class F4BakeNav : MonoBehaviour //4�� �׺�޽� ���� ����
{
    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        StartCoroutine(BakeNav());
    }

    private IEnumerator BakeNav()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("4�� �׺�޽� �ڷ�ƾ �۵�");
        yield return new WaitForSeconds(1f);
    }
}
