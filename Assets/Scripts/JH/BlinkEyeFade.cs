using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlinkEyeFade : MonoBehaviour
{
    public RectTransform topImage;
    public RectTransform bottomImage;
    public float duration = 0.6f; // 애니메이션 시간

    private Vector3 topStartPos;
    private Vector3 topTargetPos;
    private Vector3 bottomStartPos;
    private Vector3 bottomTargetPos;

    private void Start()
    {
        // 초기 위치 설정
        topStartPos = new Vector3(0f, 810f, 0f);
        topTargetPos = new Vector3(0f, 271f, 0f);

        bottomStartPos = new Vector3(0f, -810f, 0f);
        bottomTargetPos = new Vector3(0f, -271f, 0f);

        // 시작 위치 설정
        topImage.anchoredPosition = topStartPos;
        bottomImage.anchoredPosition = bottomStartPos;
    }

    public void PlayEyeBlinkEffect()
    {
        // 코루틴으로 애니메이션 실행
        StartCoroutine(SlideUIImages());
    }

    private System.Collections.IEnumerator SlideUIImages()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 슬라이딩 위치 계산
            topImage.anchoredPosition = Vector3.Lerp(topStartPos, topTargetPos, t);
            bottomImage.anchoredPosition = Vector3.Lerp(bottomStartPos, bottomTargetPos, t);

            yield return null;
        }

        // 다시 원래 위치로 돌아가는 애니메이션
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 슬라이딩 복귀 위치 계산
            topImage.anchoredPosition = Vector3.Lerp(topTargetPos, topStartPos, t);
            bottomImage.anchoredPosition = Vector3.Lerp(bottomTargetPos, bottomStartPos, t);

            yield return null;
        }
    }
}