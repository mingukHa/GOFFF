using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlinkEyeFade : MonoBehaviour
{
    public RectTransform topImage;
    public RectTransform bottomImage;
    public float duration = 0.6f; // �ִϸ��̼� �ð�

    private Vector3 topStartPos;
    private Vector3 topTargetPos;
    private Vector3 bottomStartPos;
    private Vector3 bottomTargetPos;

    private void Start()
    {
        // �ʱ� ��ġ ����
        topStartPos = new Vector3(0f, 810f, 0f);
        topTargetPos = new Vector3(0f, 271f, 0f);

        bottomStartPos = new Vector3(0f, -810f, 0f);
        bottomTargetPos = new Vector3(0f, -271f, 0f);

        // ���� ��ġ ����
        topImage.anchoredPosition = topStartPos;
        bottomImage.anchoredPosition = bottomStartPos;
    }

    public void PlayEyeBlinkEffect()
    {
        // �ڷ�ƾ���� �ִϸ��̼� ����
        StartCoroutine(SlideUIImages());
    }

    private System.Collections.IEnumerator SlideUIImages()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �����̵� ��ġ ���
            topImage.anchoredPosition = Vector3.Lerp(topStartPos, topTargetPos, t);
            bottomImage.anchoredPosition = Vector3.Lerp(bottomStartPos, bottomTargetPos, t);

            yield return null;
        }

        // �ٽ� ���� ��ġ�� ���ư��� �ִϸ��̼�
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �����̵� ���� ��ġ ���
            topImage.anchoredPosition = Vector3.Lerp(topTargetPos, topStartPos, t);
            bottomImage.anchoredPosition = Vector3.Lerp(bottomTargetPos, bottomStartPos, t);

            yield return null;
        }
    }
}