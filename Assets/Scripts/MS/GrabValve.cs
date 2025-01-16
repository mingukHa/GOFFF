using Photon.Pun;
using UnityEngine;

public class GrabValve : MonoBehaviourPun
{
    // �������� ���� �̺�Ʈ�� ���� ���
    public delegate void grabValveDelegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValveTrigger;
    public delegate void grabValve2Delegate(GameObject gameObject, Collider other);
    public grabValveDelegate grabValve2Trigger;

    private bool isGrabbed = false;

    private Vector3 currentTR = Vector3.zero;

    private void Start()
    {
        currentTR = new Vector3(-30f, 0, -27f);
    }

    private void Update()
    {
        // ��갡 �߶��� ��� currentTr ��ҿ� ����
        if(-15f>transform.position.y)
        {
            Rigidbody ValveRD=transform.GetComponent<Rigidbody>();
            ValveRD.angularVelocity = Vector3.zero;
            ValveRD.linearVelocity = Vector3.zero;
            transform.position = currentTR;
            transform.rotation = Quaternion.identity;
        }
    }

    // �ٸ� Collider�� �� ���� �浹���� �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if (isGrabbed)
        {
            if (other.name == "CylinderA")
            {
                grabValveTrigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger", RpcTarget.Others ,other);
            }
            else if (other.name == "CylinderB")
            {
                grabValve2Trigger?.Invoke(gameObject, other);
                photonView.RPC("RPCTrigger2", RpcTarget.Others, other);
            }
        }
    }

    [PunRPC]
    private void RPCTrigger(Collider other)
    {
        grabValveTrigger?.Invoke(gameObject, other);
    }

    [PunRPC]
    private void RPCTrigger2(Collider other)
    {
        grabValve2Trigger?.Invoke(gameObject, other);
    }

    public void SelectOn()
    {
        Debug.Log("��긦 ��ҽ��ϴ�.");
        isGrabbed = true;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, true);

    }

    public void SelectOff()
    {
        Debug.Log("��긦 ���ҽ��ϴ�.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }

    public void OnSelectEnter()
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    // grab�� ������쿡 isKinematic�� �ٸ� Ŭ���̾�Ʈ������ �������� �ʾ�
    // �߷��� ������ �ޱ� ������ ����� ��� �ٸ� Ŭ���̾�Ʈ���� isKinematic�� �������� ��
    [PunRPC]
    private void RPCGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
        Rigidbody grabValveRD = transform.GetComponent<Rigidbody>();
        if (grabValveRD != null)
        {
            grabValveRD.isKinematic = grabbed;
        }
    }

}
