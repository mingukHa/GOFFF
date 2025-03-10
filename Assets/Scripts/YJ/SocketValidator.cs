using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Photon.Pun;

public class SocketValidator : MonoBehaviourPun
{
    public string expectedObjectName; // 이 Socket에 배치되어야 할 Object의 이름
    private XRSocketInteractor socketInteractor;
    private GameObject placedObject;
    private GameObject[] zomshout;

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
        // 소켓에 들어갔을 때 효과음
        SoundManager.instance.SFXPlay("SocketItem_SFX", this.gameObject);

        placedObject = args.interactableObject.transform.gameObject;

        if (placedObject.name == expectedObjectName)
        {
            Debug.Log($"오브젝트가 올바른 위치에 있음: {placedObject.name}");
        }
        else
        {
            Debug.LogWarning($"오브젝트가 잘못된 위치에 있음: {placedObject.name}");
            socketInteractor.enabled = false;
            socketInteractor.enabled = true;
        }

        // 퍼즐 상태 확인
        photonView.RPC("UpdatePuzzleStatus", RpcTarget.All);
    }

    public void OnObjectRemoved(SelectExitEventArgs args)
    {
        Debug.Log($"Object removed: {args.interactableObject.transform.gameObject.name}");
        placedObject = null;

        // 퍼즐 상태 확인
        photonView.RPC("UpdatePuzzleStatus", RpcTarget.All);
    }

    [PunRPC]
    public void UpdatePuzzleStatus()
    {
        puzzleManager?.CheckPuzzleStatus();
    }

    public bool IsObjectCorrectlyPlaced()
    {
        return placedObject != null && placedObject.name == expectedObjectName;
    }
}
