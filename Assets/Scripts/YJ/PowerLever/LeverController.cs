using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LeverController : MonoBehaviourPun
{
    // 핸들, 엘리베이터 오브젝트
    public Transform handle; // 레버 핸들
    public BoxCollider eVButton; // 엘리베이터 상승 버튼

    // 레버 올리는 동작
    private Vector3 targetRotation = new Vector3(-45, 0, 0); // 올라간 레버의 회전값
    private float rotationDuration = 1.0f; // 레버가 올라가는 속도

    // 레버가 올라갔는지 여부 확인
    private bool isActivated = false;

    public void ActivateLever()
    {
        if (isActivated) return; // 레버가 1회만 작동하도록 함

        isActivated = true;

        // 레버 회전
        if (handle != null)
        {
            photonView.RPC("RPC_RotateHandle", RpcTarget.All);
        }

        // 엘리베이터 버튼 활성화
        photonView.RPC("RPC_ActivateEVButton", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_RotateHandle()
    {
        StartCoroutine(RotateHandle());
    }

    private IEnumerator RotateHandle()
    {
        Quaternion initialRotation = handle.localRotation;
        Quaternion finalRotation = Quaternion.Euler(targetRotation);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            handle.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        handle.localRotation = finalRotation; // 최종 회전값 확인
    }

    [PunRPC]
    private void RPC_ActivateEVButton()
    {
        eVButton.enabled = true;
    }
}
