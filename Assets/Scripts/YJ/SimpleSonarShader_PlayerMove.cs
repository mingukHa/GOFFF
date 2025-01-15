using System.Collections;
using UnityEngine;
using Photon.Pun;

public class SimpleSonarShader_PlayerMove : MonoBehaviourPun
{
    SimpleSonarShader_Parent par = null;

    private Transform playerTransform;
    private Vector3 lastPosition;
    [SerializeField]
    private float speedThreshold = 1.0f; // �ӵ� �Ӱ谪
    private float sonarCooldown = 1.0f; // Sonar ȿ�� ��ٿ� �ð�
    private float lastSonarTime = -1.0f;

    public delegate void CollisionEvent(Vector3 collisionPoint);
    public static event CollisionEvent PlayerMove;

    private IEnumerator Start()
    {
        //par = GetComponentInParent<SimpleSonarShader_Parent>();
        par = FindFirstObjectByType<SimpleSonarShader_Parent>();

        // Player �˻��� ���� �ð� ���� �ݺ� �õ�
        GameObject playerInstance = null;
        while (playerInstance == null)
        {
            playerInstance = GameObject.FindWithTag("Player");
            yield return null; // ���� �����ӱ��� ���
        }

        playerTransform = playerInstance.transform;
        lastPosition = playerTransform.position;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // �̵� �ӵ� ���
        Vector3 currentPosition = playerTransform.position;
        float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
        lastPosition = currentPosition;
        //Debug.Log("���� �ӵ�:" + speed);

        // �ӵ��� �Ӱ谪 �̻��� �� Sonar �ߵ�
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

            // PlayerHolder�� �߽����� Sonar ȿ�� �ߵ�
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
