using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Domwaiter : MonoBehaviourPunCallbacks
{
    // 핸들, 엘리베이터 오브젝트
    public Transform handle;
    public Transform DomwaiterBottom;

    // 문 열림/닫힘 각도
    public float openAngle = -90f; // 열린 문 회전값

    // 레버 올리는 동작
    private Vector3 openrotation;
    private Vector3 closerotation = Vector3.zero;   // 닫힌 문 회전값

    private float rotationDuration = 1.0f; // 문이 돌아가는 시간

    // 레버가 올라갔는지 여부 확인
    private bool isOpened = false;

    private Quaternion localRotation;   // 문의 회전

    private void Start()
    {
        // 열린 문 각도 초기화
        openrotation = new Vector3(0, 0, openAngle);
    }

    public void Activatehandle()
    {
        if (isOpened)
        {
            isOpened = false;
            // 로컬에서 문을 열려있으면 닫고, 네트워크에서도 동기화
            RotateDoorLocally(closerotation); // 로컬에서 문 닫기
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
        else
        {
            isOpened = true;
            // 로컬에서 문을 닫혀있으면 열고, 네트워크에서도 동기화
            RotateDoorLocally(openrotation); // 로컬에서 문 닫기
            photonView.RPC("RotateDoorRPC", RpcTarget.All, openrotation);
        }
    }

    private void RotateDoorLocally(Vector3 targetRotation)
    {
        StartCoroutine(RotateDoor(targetRotation));
    }

    [PunRPC]
    public void RotateDoorRPC(Vector3 targetRotation)
    {
        RotateDoorLocally(targetRotation);
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

        while (elapsedTime < rotationDuration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsedTime / rotationDuration);
            transform.rotation = Quaternion.Lerp(initialRotation, targetQuaternion, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 코루틴이 끝난 후 최종 회전 값 설정
        transform.rotation = targetQuaternion;
    }
}