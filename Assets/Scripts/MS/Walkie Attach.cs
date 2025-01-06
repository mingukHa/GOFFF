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
        if (!photonView.IsMine && isHaved) return; // �����ָ� �̺�Ʈ ó��

        Debug.Log("���� ĳ��Ʈ�� ��");

        // ���ÿ��� ��ġ �̵� ó��
        //transform.SetParent(attachPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isSelected = true;
        isHaved = true;

        // �ٸ� Ŭ���̾�Ʈ�鿡�� ��ġ�� ȸ�� ����
        photonView.RPC(nameof(RPC_UpdateWalkiePosition), RpcTarget.Others, attachPoint.name);
    }

    [PunRPC]
    private void RPC_UpdateWalkiePosition(string attachPointName)
    {
        // attachPoint�� ã�� (�ٸ� Ŭ���̾�Ʈ�� ������ Transform�� ������ ��)
        Transform newAttachPoint = GameObject.Find(attachPointName)?.transform;

        if (newAttachPoint != null)
        {
            //transform.SetParent(newAttachPoint, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
