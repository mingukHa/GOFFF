using UnityEngine;
using Photon.Pun;

public class WalkieAttach : MonoBehaviour
{
    [SerializeField]
    private Transform attachPoint = null;
    private bool isSelected = false;
    private bool isHaved = false;

    public bool IsSelected { get { return isSelected; } }

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void OnSelectWalkie()
    {
        if (!photonView.IsMine && isHaved) return; // 소유주만 이벤트 처리

        Debug.Log("레이 캐스트가 됨");

        // 로컬에서 위치 이동 처리
        //transform.SetParent(attachPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isSelected = true;
        isHaved = true;

        // 다른 클라이언트들에게 위치와 회전 전송
        photonView.RPC(nameof(RPC_UpdateWalkiePosition), RpcTarget.Others, attachPoint.name);
    }

    [PunRPC]
    private void RPC_UpdateWalkiePosition(string attachPointName)
    {
        // attachPoint를 찾음 (다른 클라이언트도 동일한 Transform을 가져야 함)
        Transform newAttachPoint = GameObject.Find(attachPointName)?.transform;

        if (newAttachPoint != null)
        {
            //transform.SetParent(newAttachPoint, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
