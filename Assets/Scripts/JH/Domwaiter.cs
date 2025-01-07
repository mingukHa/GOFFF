using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Domwaiter : MonoBehaviourPunCallbacks
{
    // 핸들, 엘리베이터 오브젝트
    public Transform handle;
    public Transform DomwaiterBottom;

    // 레버 올리는 동작
    private Vector3 openrotation = new Vector3(0, 0, -90); // 열린 문 회전값
    private Vector3 closerotation = new Vector3(0, 0, 0);   // 닫힌 문 회전값
    private float rotationDuration = 1.0f; // 문이 돌아가는 시간

    // 레버가 올라갔는지 여부 확인
    private bool isOpened = false;

    private Quaternion localRotation;   // 문의 회전

    public void Activatehandle()
    {
        SoundManager.instance.SFXPlay("ElevatorDoor2_SFX");

        if (isOpened)
        {
            isOpened = false;
            // 로컬에서 문을 닫고, 네트워크에서도 동기화
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
        else
        {
            isOpened = true;
            // 로컬에서 문을 닫고, 네트워크에서도 동기화
            photonView.RPC("RotateDoorRPC", RpcTarget.All, closerotation);
        }
    }

    [PunRPC]
    public void RotateDoorRPC(Vector3 targetRotation)
    {
        StartCoroutine(RotateDoor(targetRotation));
    }

    private IEnumerator RotateDoor(Vector3 targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            localRotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(targetRotation), t);
            // 회전값을 적용
            transform.rotation = localRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전이 끝난 후 최종 회전값을 정확히 설정
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}