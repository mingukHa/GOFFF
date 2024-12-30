using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketValidator : MonoBehaviour
{
    public string expectedObjectName; // 이 Socket에 배치되어야 할 Object의 이름
    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        // 이벤트를 코드로 연결 (선택 사항)
        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
        socketInteractor.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDestroy()
    {
        socketInteractor.selectEntered.RemoveListener(OnObjectPlaced);
        socketInteractor.selectExited.RemoveListener(OnObjectRemoved);
    }

    // **Select Entered** 이벤트에 연결되는 메서드
    public void OnObjectPlaced(SelectEnterEventArgs args)
    {
        // Socket에 배치된 Object 가져오기
        GameObject placedObject = args.interactableObject.transform.gameObject;

        // Object 검증
        if (placedObject.name == expectedObjectName)
        {
            Debug.Log($"Correct object placed: {placedObject.name}");
            // 추가 게임 로직 실행 (예: 점수 업데이트, 퍼즐 진행 상태 변경)
        }
        else
        {
            Debug.LogWarning($"Incorrect object placed: {placedObject.name}");
        }
    }

    // **Select Exited** 이벤트에 연결되는 메서드 (선택 사항)
    public void OnObjectRemoved(SelectExitEventArgs args)
    {
        GameObject removedObject = args.interactableObject.transform.gameObject;
        Debug.Log($"Object removed: {removedObject.name}");
        // 추가 로직 실행 (예: 퍼즐 상태 초기화)
    }
}
