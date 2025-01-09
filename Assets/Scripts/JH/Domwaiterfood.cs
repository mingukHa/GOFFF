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
        // 버튼이 눌렸는지 확인
        if (isButtonPressed)
        {
            // 2번 박스(ovenbottom[0])의 위치에서 충돌한 모든 오브젝트 찾기
            Collider[] colliders = Physics.OverlapBox(
                ovenbottom[0].position,
                ovenbottom[0].localScale / 2,
                Quaternion.identity
            );

            foreach (Collider col in colliders)
            {
                // 충돌한 오브젝트가 버튼이 아닌 경우
                if (col.gameObject != telpobtn)
                {
                    // 오브젝트를 ovenbottom[1] 위치로 이동
                    col.transform.position = ovenbottom[1].position;
                }
            }

            yield return null;
        }
    }

    // 버튼이 눌렸을 때 isButtonPressed를 true로 설정
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == telpobtn) // 버튼이 눌리면
        {
            isButtonPressed = true;
        }
    }

    // 버튼이 떼어졌을 때 isButtonPressed를 false로 설정
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == telpobtn)
        {
            isButtonPressed = false;
        }
    }
}