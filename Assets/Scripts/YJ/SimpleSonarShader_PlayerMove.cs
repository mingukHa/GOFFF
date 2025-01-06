using System.Collections;
using UnityEngine;
using Photon.Pun;

public class SimpleSonarShader_PlayerMove : MonoBehaviourPun
{
    SimpleSonarShader_Parent par = null;

    private Transform playerTransform;
    private Vector3 lastPosition;
    [SerializeField]
    private float speedThreshold = 1.0f; // 속도 임계값
    private float sonarCooldown = 1.0f; // Sonar 효과 쿨다운 시간
    private float lastSonarTime = -1.0f;

    private IEnumerator Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();

        // Player 검색을 일정 시간 동안 반복 시도
        GameObject playerInstance = null;
        while (playerInstance == null)
        {
            playerInstance = GameObject.FindWithTag("Player");
            yield return null; // 다음 프레임까지 대기
        }

        playerTransform = playerInstance.transform;
        lastPosition = playerTransform.position;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 이동 속도 계산
        Vector3 currentPosition = playerTransform.position;
        float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
        lastPosition = currentPosition;
        Debug.Log("현재 속도:" + speed);

        // 속도가 임계값 이상일 때 Sonar 발동
        if (speed > speedThreshold && Time.time - lastSonarTime > sonarCooldown)
        {
            TriggerSonar();
            lastSonarTime = Time.time;
        }
    }

    private void TriggerSonar()
    {
        if (par)
        {
            Vector3 sonarOrigin = playerTransform.position;

            // PlayerHolder를 중심으로 Sonar 효과 발동
            par.StartSonarRing(sonarOrigin, 1.4f, 0);

            // Sonar 효과 발생 위치 주변 충돌 탐지
            float detectionRadius = 0.1f; // 탐지 반경 설정
            Collider[] hitColliders = Physics.OverlapSphere(sonarOrigin, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                // 충돌한 오브젝트 처리
                Debug.Log("충돌한 오브젝트: " + hitCollider.name);

                //// OnCollisionEnter와 유사한 동작을 호출 (예: 충돌한 오브젝트에 특수 효과 적용)
                //if (par)
                //{
                //    par.StartSonarRing(hitCollider.ClosestPoint(sonarOrigin), 1.4f, 0);
                //}
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 Sonar 효과
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
