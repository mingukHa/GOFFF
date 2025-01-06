using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class WalkieAttach : MonoBehaviourPun
{
    //[SerializeField]
    //private Transform attachPoint = null;
    private bool isSelected = false;
    private bool isHaved = false;

    public bool IsSelected { get { return isSelected; } }


    private void Awake()
    {
        //photonView = GetComponent<PhotonView>();
    }

    // RPC로 물체 위치 동기화
    [PunRPC]
    public void SyncObjectPosition(Transform grabbedTr, Vector3 position, Vector3 localPosition, Quaternion localRotation)
    {
        if (photonView.IsMine)
        {
            // playerHolder를 찾아서 위치 설정
            //GameObject playerHolder = GameObject.Find("PlayerHolder"); // 플레이어의 빈 오브젝트를 이름으로 찾기
            if (grabbedTr != null)
            {
                grabbedTr.transform.position = position;
                grabbedTr.transform.localPosition = localPosition;
                grabbedTr.localRotation = localRotation;
            }
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (photonView.IsMine)
        {
            Transform grabbedTr = args.interactableObject.transform;
            if (grabbedTr != null)
            {
                // playerHolder를 찾고, 물체를 해당 자식으로 설정
                GameObject playerHolder = GameObject.Find("Walkie Attach"); // 플레이어의 빈 오브젝트를 찾음
                if (playerHolder != null)
                {
                    isSelected = true;

                    grabbedTr.SetParent(playerHolder.transform, false);
                    grabbedTr.localPosition = Vector3.zero;
                    grabbedTr.localRotation = Quaternion.identity;

                    // 물체의 위치 동기화 요청 (RPC)
                    photonView.RPC("SyncObjectPosition", RpcTarget.AllBuffered, grabbedTr, grabbedTr.position, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("PlayerHolder를 찾을 수 없습니다.");
                }
            }
        }
    }



    //public void OnSelectWalkie(SelectEnterEventArgs args)
    //{
    //    if (!photonView.IsMine && isHaved || !PhotonNetwork.IsConnected ) return; // 소유주만 이벤트 처리


    //    PhotonView objectView = obj.GetComponent<PhotonView>();

    //    isSelected = true;
    //    isHaved = true;

    //    // 다른 클라이언트들에게 위치와 회전 전송
    //    photonView.RPC(nameof(RPC_UpdateWalkiePosition), RpcTarget.Others, attachPoint.name);
    //}

    //[PunRPC]
    //private void RPC_UpdateWalkiePosition(int playerViewID, int objectViewID)
    //{
    //    // 플레이어와 물건 찾기
    //    PhotonView playerView = PhotonView.Find(playerViewID);
    //    PhotonView objectView = PhotonView.Find(objectViewID);

    //    if (playerView != null && objectView != null)
    //    {
    //        GameObject playerHolder = playerView.GetComponent<PlayerManager>().playerHolder;

    //        // 물건을 플레이어의 빈 오브젝트로 이동
    //        objectView.transform.SetParent(playerHolder.transform);
    //        transform.localPosition = Vector3.zero;
    //        transform.localRotation = Quaternion.identity;
    //    }
    //}
}
