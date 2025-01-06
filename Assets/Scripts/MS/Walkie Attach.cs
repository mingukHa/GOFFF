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

    // RPC�� ��ü ��ġ ����ȭ
    [PunRPC]
    public void SyncObjectPosition(Transform grabbedTr, Vector3 position, Vector3 localPosition, Quaternion localRotation)
    {
        if (photonView.IsMine)
        {
            // playerHolder�� ã�Ƽ� ��ġ ����
            //GameObject playerHolder = GameObject.Find("PlayerHolder"); // �÷��̾��� �� ������Ʈ�� �̸����� ã��
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
                // playerHolder�� ã��, ��ü�� �ش� �ڽ����� ����
                GameObject playerHolder = GameObject.Find("Walkie Attach"); // �÷��̾��� �� ������Ʈ�� ã��
                if (playerHolder != null)
                {
                    isSelected = true;

                    grabbedTr.SetParent(playerHolder.transform, false);
                    grabbedTr.localPosition = Vector3.zero;
                    grabbedTr.localRotation = Quaternion.identity;

                    // ��ü�� ��ġ ����ȭ ��û (RPC)
                    photonView.RPC("SyncObjectPosition", RpcTarget.AllBuffered, grabbedTr, grabbedTr.position, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("PlayerHolder�� ã�� �� �����ϴ�.");
                }
            }
        }
    }



    //public void OnSelectWalkie(SelectEnterEventArgs args)
    //{
    //    if (!photonView.IsMine && isHaved || !PhotonNetwork.IsConnected ) return; // �����ָ� �̺�Ʈ ó��


    //    PhotonView objectView = obj.GetComponent<PhotonView>();

    //    isSelected = true;
    //    isHaved = true;

    //    // �ٸ� Ŭ���̾�Ʈ�鿡�� ��ġ�� ȸ�� ����
    //    photonView.RPC(nameof(RPC_UpdateWalkiePosition), RpcTarget.Others, attachPoint.name);
    //}

    //[PunRPC]
    //private void RPC_UpdateWalkiePosition(int playerViewID, int objectViewID)
    //{
    //    // �÷��̾�� ���� ã��
    //    PhotonView playerView = PhotonView.Find(playerViewID);
    //    PhotonView objectView = PhotonView.Find(objectViewID);

    //    if (playerView != null && objectView != null)
    //    {
    //        GameObject playerHolder = playerView.GetComponent<PlayerManager>().playerHolder;

    //        // ������ �÷��̾��� �� ������Ʈ�� �̵�
    //        objectView.transform.SetParent(playerHolder.transform);
    //        transform.localPosition = Vector3.zero;
    //        transform.localRotation = Quaternion.identity;
    //    }
    //}
}
