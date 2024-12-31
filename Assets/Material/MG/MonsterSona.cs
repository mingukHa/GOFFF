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
