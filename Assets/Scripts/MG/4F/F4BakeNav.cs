using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class F4BakeNav : MonoBehaviour //4층 네브메쉬 동적 생성
{
    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        StartCoroutine(BakeNav());
    }

    private IEnumerator BakeNav()
    {
        navMeshSurface.BuildNavMesh();
        Debug.Log("4층 네브메쉬 코루틴 작동");
        yield return new WaitForSeconds(1f);
    }
}
