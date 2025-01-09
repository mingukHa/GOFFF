using System.Collections;
using UnityEngine;

public class Domwaiterfood : MonoBehaviour
{
    public Transform[] ovenbottom;

    private bool isButtonPressed = false;

    public void OnButtonPressed()
    {
        isButtonPressed = true;
        telpo();
    }

    // XR Simple Interactable의 Select Exit 이벤트에 연결할 메서드
    public void OnButtonReleased()
    {
        isButtonPressed = false;
    }

    public void telpo()
    {
        StartCoroutine(Telpoburgur());
    }

    private IEnumerator Telpoburgur()
    {
        if (isButtonPressed)
        {
            // ovenbottom[0]의 충돌 박스 크기를 설정
            Vector3 boxSize = new Vector3(1f, 1f, 1f); // 원하는 크기로 조절

            // ovenbottom[0]의 박스 충돌체 안에 있는 모든 오브젝트 검색
            Collider[] colliders = Physics.OverlapBox(ovenbottom[0].position, boxSize / 2);

            // 검색된 모든 오브젝트를 ovenbottom[1] 위치로 이동
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Food")) // 특정 태그의 오브젝트만 이동
                {
                    col.transform.position = ovenbottom[1].position;
                }
            }

            yield return null;
        }
    }

    // 충돌 박스를 시각화하여 확인할 수 있도록 OnDrawGizmos 추가
    private void OnDrawGizmos()
    {
        if (ovenbottom != null && ovenbottom.Length > 0)
        {
            Gizmos.color = Color.red;
            Vector3 boxSize = new Vector3(1f, 1f, 1f);
            Gizmos.DrawWireCube(ovenbottom[0].position, boxSize);
        }
    }
}