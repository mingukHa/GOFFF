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

    public delegate void CollisionEvent(Vector3 collisionPoint);
    public static event CollisionEvent PlayerMove;

    private IEnumerator Start()
    {
        //par = GetComponentInParent<SimpleSonarShader_Parent>();
        par = FindFirstObjectByType<SimpleSonarShader_Parent>();

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
        //Debug.Log("현재 속도:" + speed);

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
            SoundManager.instance.SFXPlay("Wheel_SFX", this.gameObject);
            Vector3 sonarOrigin = playerTransform.position;

            // PlayerHolder를 중심으로 Sonar 효과 발동
            par.StartSonarRing(sonarOrigin, 1.2f, 0);
            PlayerMove?.Invoke(sonarOrigin);
            photonView.RPC("RPCPlayerMove", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RPCPlayerMove(Vector3 sonarOrigin)
    {
        PlayerMove?.Invoke(sonarOrigin);
    }
}
