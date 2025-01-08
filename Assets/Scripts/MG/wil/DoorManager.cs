using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false; // 문 상태 (열림/닫힘)
    public float openAngle = -90f; // 문이 열릴 각도 (예: 90도 또는 -90도)
    public float animationTime = 1f; // 문 열림/닫힘 애니메이션 시간

    private Quaternion closedRotation; // 닫힌 상태의 회전값
    private Quaternion openRotation; // 열린 상태의 회전값

    void Start()
    {
        // 초기 회전값 설정
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    // 문 열기/닫기 토글
    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
            Debug.Log("닫힘");
        }
        else
        {
            OpenDoor();
            Debug.Log("열림");
        }
    }

    // 문 열기
    public void OpenDoor()
    {
        SoundManager.instance.SFXPlay("OpenDoor2_SFX");
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(openRotation));
        isOpen = true;
    }

    // 문 닫기
    private void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateDoor(closedRotation));
        isOpen = false;
    }

    // 문 애니메이션 처리
    private System.Collections.IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        float elapsedTime = 0;
        Quaternion startingRotation = transform.rotation;

        while (elapsedTime < animationTime)
        {
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
