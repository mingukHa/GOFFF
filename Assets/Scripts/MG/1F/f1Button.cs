using UnityEngine;
using System.Collections;
using Photon.Pun;
public class F1Button : MonoBehaviourPun //1층 버튼 관리
{
    public float MoveSpeed = 5f; // 이동 속도
    public GameObject door;
    public GameObject target;

    private Coroutine moveCoroutine; // 이동 Coroutine 참조
    [PunRPC]
    public void ButtonMove()
    {
        // 이미 실행 중인 Coroutine이 있다면 멈춤
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        // 새로운 Coroutine 시작
        SoundManager.instance.SFXPlay("Button_SFX", this.gameObject);
        SoundManager.instance.SFXPlay("Shutter_SFX", this.gameObject);
        moveCoroutine = StartCoroutine(MoveDoor());
        photonView.RPC("ButtonMove", RpcTarget.Others);
    }
    
    private IEnumerator MoveDoor()
    {
        while (Vector3.Distance(door.transform.position, target.transform.position) > 0.01f)
        {
            // 부드럽게 목표 위치로 이동
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                target.transform.position,
                MoveSpeed * Time.deltaTime
            );

            yield return null; // 다음 프레임까지 대기
        }

        // 이동 완료 후 정확히 목표 위치로 설정
        door.transform.position = target.transform.position;

        // Coroutine 참조 초기화
        moveCoroutine = null;
    }
}
