using UnityEngine;
using Photon.Pun;
using System.Collections;

public class Domwaiter : MonoBehaviourPun
{
    public Transform ovenDoor;  // 오븐 문 오브젝트
    private bool isOpen = false;    
    private bool isRotating = false; // 회전 중이면 추가 트리거를 방지
    public float rotationDuration = 1f;  // 문이 회전하는 시간 (인스펙터에서 조정 가능)

    public void ToggleDoor()
    {
        if (isRotating) return;  // 문이 회전 중이면 토글하지 않음

        photonView.RPC("RPCToggleDoor", RpcTarget.All);
    }

    // 네트워크로 호출되는 문 상태 변경 함수 (RPC)
    [PunRPC]
    public void RPCToggleDoor()
    {
        // 현재 문 각도 체크
        float targetRotation = isOpen ? 0f : 90f;
        if (Mathf.Approximately(ovenDoor.localRotation.eulerAngles.y, targetRotation)) return;

        StartCoroutine(RotateDoor(isOpen ? 0f : 90f));  // 열려 있으면 닫기, 아니면 열기

        SoundManager.instance.SFXPlay("OpenDoor_SFX");  // 문 열림 사운드 재생
    }

    // 문 회전 코루틴
    private IEnumerator RotateDoor(float targetRotation)
    {
        //회전하겠다!
        isRotating = true;

        //회전 초기, 결과, 시간 비교값
        Quaternion startRotation = ovenDoor.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, targetRotation, 0f);
        float timeElapsed = 0f;
        
        //회전
        while (timeElapsed < rotationDuration)
        {
            ovenDoor.localRotation = Quaternion.Slerp
                (startRotation, endRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;  // 한 프레임 대기
        }

        ovenDoor.localRotation = endRotation;  // 최종 회전값 적용
        isRotating = false;  // 회전 완료
        isOpen = !isOpen;  // 상태 변경을 회전 완료 후에 수행
    }
}