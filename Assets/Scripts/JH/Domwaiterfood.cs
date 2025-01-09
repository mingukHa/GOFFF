using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    public Transform[] ovenbottom;
    public GameObject telpobtn;

    private bool isButtonPressed = false;

    public void telpo()
    {
        StartCoroutine(Telpoburgur());
    }

    private IEnumerator Telpoburgur()
    {
        // 버튼이 눌렸는지 확인 (여기서는 isButtonPressed 변수로 상태를 관리)
        if (isButtonPressed)
        {
            // 2번 박스(ovenbottom[0])와 충돌한 모든 오브젝트들 찾기
            Collider[] colliders = Physics.OverlapBox(ovenbottom[0].position, ovenbottom[0].localScale / 2);

            foreach (Collider col in colliders)
            {
                // 충돌한 오브젝트가 버튼이 아닌 다른 오브젝트라면
                if (col.gameObject != telpobtn)
                {
                    // ovenbottom[1] 위치로 오브젝트 이동
                    col.transform.position = ovenbottom[1].position;
                }
            }

            // 코루틴 종료
            yield return null;
        }
    }

    // 버튼이 눌렸을 때 isButtonPressed를 true로 설정
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == telpobtn) // 버튼이 눌리면
        {
            isButtonPressed = true;
        }
    }

    // 버튼이 떼어졌을 때 isButtonPressed를 false로 설정
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == telpobtn)
        {
            isButtonPressed = false;
        }
    }
}