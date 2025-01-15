using System.Collections;
using UnityEngine;
using Photon.Pun;

public class LeverController : MonoBehaviourPun
{
    public Transform lever; // 레버
    public BoxCollider eVButton; // 엘리베이터 상승 버튼 콜라이더

    // 레버 올리는 동작
    private Vector3 targetRotation = new Vector3(-45, 0, 0); // 올라간 레버의 회전값
    private float rotationDuration = 1.0f; // 레버 상승 소요 시간

    // 레버가 올라갔는지 여부 확인
    private bool isActivated = false;

    // 레버의 Simple Interactor가 작동하면 실행
    public void ActivateLever()
    {
        // 레버가 이미 작동했다면 return
        if (isActivated) return;

        // 레버가 1회만 작동하도록 상태를 true로 변경
        isActivated = true;

        // RPC로 RPC_RotateHandle 메소드 호출
        if (lever != null)
        {
            photonView.RPC("RPC_RotateHandle", RpcTarget.All);
        }

        // RPC로 RPC_ActivateEVButton 메소드 호출
        if (eVButton != null)
        {
            photonView.RPC("RPC_ActivateEVButton", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_RotateLever()
    {
        StartCoroutine(RotateLever());
    }

    private IEnumerator RotateLever()
    {
        Quaternion initialRotation = lever.localRotation; // 레버 초기 회전값
        Quaternion finalRotation = Quaternion.Euler(targetRotation); // 올라간 레버 회전값

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            // 레버 상승 소요 시간만큼 Slerp로 보간하여 부드러운 움직임 구현
            lever.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lever.localRotation = finalRotation; // 레버의 회전값을 최종 회전값과 동기화
        SoundManager.instance.SFXPlay("Spark1_SFX", this.gameObject);
    }

    [PunRPC]
    private void RPC_ActivateEVButton()
    {
        // 엘리베이터 상승 버튼 콜라이더 활성화
        eVButton.enabled = true;
    }
}
