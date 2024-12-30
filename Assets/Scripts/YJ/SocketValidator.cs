using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketValidator : MonoBehaviour
{
    public string expectedObjectName; // 이 Socket에 배치되어야 할 Object의 이름
    private XRSocketInteractor socketInteractor;
    private GameObject placedObject;

    public PuzzleManager puzzleManager; // PuzzleManager 참조

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        // 이벤트 연결
        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
        socketInteractor.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDestroy()
    {
        socketInteractor.selectEntered.RemoveListener(OnObjectPlaced);
        socketInteractor.selectExited.RemoveListener(OnObjectRemoved);
    }

    public void OnObjectPlaced(SelectEnterEventArgs args)
    {
        placedObject = args.interactableObject.transform.gameObject;

        if (placedObject.name == expectedObjectName)
        {
            Debug.Log($"Correct object placed: {placedObject.name}");
        }
        else
        {
            Debug.LogWarning($"Incorrect object placed: {placedObject.name}");
        }

        // 퍼즐 상태 확인
        puzzleManager?.CheckPuzzleStatus();
    }

    public void OnObjectRemoved(SelectExitEventArgs args)
    {
        Debug.Log($"Object removed: {args.interactableObject.transform.gameObject.name}");
        placedObject = null;

        // 퍼즐 상태 확인
        puzzleManager?.CheckPuzzleStatus();
    }

    public bool IsObjectCorrectlyPlaced()
    {
        return placedObject != null && placedObject.name == expectedObjectName;
    }
}
