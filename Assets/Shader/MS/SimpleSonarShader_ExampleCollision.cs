using UnityEngine;
using Photon.Pun;

public class SimpleSonarShader_ExampleCollision : MonoBehaviourPun
{
    SimpleSonarShader_Parent par = null;

    private void Start()
    {
        par = GetComponentInParent<SimpleSonarShader_Parent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Master Client만 이벤트를 처리하도록 함
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (collision.gameObject.CompareTag("Player"))
            return;

        int colliderID = collision.collider.GetInstanceID();

        // 매니저에서 쿨타임 체크
        if (CollisionCooldownManager.Instance.IsCooldownActive(colliderID))
            return; // 쿨다운 중이면 종료

        // 소나 링 시작
        if (par)
        {
            par.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude / 10.0f, 0);
        }
    }
}
