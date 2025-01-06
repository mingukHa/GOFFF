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

    private IEnumerator Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();

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
        Debug.Log("���� �ӵ�:" + speed);

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
            Vector3 sonarOrigin = playerTransform.position;

            // PlayerHolder�� �߽����� Sonar ȿ�� �ߵ�
            par.StartSonarRing(sonarOrigin, 1.4f, 0);

            // Sonar ȿ�� �߻� ��ġ �ֺ� �浹 Ž��
            float detectionRadius = 0.1f; // Ž�� �ݰ� ����
            Collider[] hitColliders = Physics.OverlapSphere(sonarOrigin, detectionRadius);

            foreach (var hitCollider in hitColliders)
            {
                // �浹�� ������Ʈ ó��
                Debug.Log("�浹�� ������Ʈ: " + hitCollider.name);

                //// OnCollisionEnter�� ������ ������ ȣ�� (��: �浹�� ������Ʈ�� Ư�� ȿ�� ����)
                //if (par)
                //{
                //    par.StartSonarRing(hitCollider.ClosestPoint(sonarOrigin), 1.4f, 0);
                //}
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹 �� Sonar ȿ��
        if (par) par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
    }
}
