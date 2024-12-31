using UnityEngine;
using System.Collections;

public class MonsterSona : MonoBehaviour
{
    [SerializeField]
    private SimpleSonarShader_Parent par; // Inspector���� ���� ����
    //private bool monster = true;

    private void Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(Monstersona());
    }

    private IEnumerator Monstersona()
    {
        Debug.Log("�ڷ�ƾ ����");
        while (true)
        {
            Vector4 position = new Vector4(transform.position.x, transform.position.y, transform.position.z, 0);
            if (par) par.StartSonarRing(position, 1.0f, 1);
            Debug.Log("Sonar Ring ����: " + transform.position);

            // 1�� ���
            yield return new WaitForSeconds(1f);
        }
    }
}
