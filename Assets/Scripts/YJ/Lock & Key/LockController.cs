using UnityEngine;

public class LockController : MonoBehaviour
{
    // DoorController를 참조할 필드
    public DoorController doorController;

    private Collider[] lockColliders;

    void Start()
    {
        // Lock 오브젝트의 하위 Collider를 가져옵니다
        lockColliders = GetComponentsInChildren<Collider>();

        // 각 Collider에 Trigger 이벤트 처리를 위한 설정
        foreach (var collider in lockColliders)
        {
            if (!collider.isTrigger)
            {
                collider.isTrigger = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 'Key' 태그를 가지고 있는지 확인
        if (other.CompareTag("Key"))
        {
            // DoorController의 OpenDoor 메서드 호출
            if (doorController != null)
            {
                doorController.OpenDoor();
            }

            // Lock 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
