using Photon.Pun;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectMain : MonoBehaviourPun
{
    private bool isGrabbed = false; //�׷� ����

    public void OnSelectEnter() //��������
    {
        if(!photonView.IsMine) //������ �ƴϸ�
        {
            photonView.RequestOwnership(); //������ �Ѱܹ���
        }
        isGrabbed = true;
        Destroy(GetComponent<FixedJoint>());
        photonView.RPC("RPCGrabbed", RpcTarget.Others, true);
        Debug.Log("������Ʈ�� ������");
    }
    public void OnSelectFalse()
    {
        Debug.Log("��ü�� ���ҽ��ϴ�.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ʈ���� �ߵ�");
        //if (!photonView.IsMine)
        //{
        //    photonView.RequestOwnership();
        //}
        if (!photonView.IsMine)
        {
            Debug.Log("�� ��ü�� �ƴ϶� Ʈ���Ű� �ƴ϶� �۵�����");
        }
            
        if(!isGrabbed && gameObject.GetComponent<FixedJoint>() == null)
        {
            if (other.CompareTag("Basket"))
            {
                IntersectionBoundsMove(other);
            }
        }
        Debug.Log("������Ʈ�� �����");
    }

    private void IntersectionBoundsMove(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // �� �ݶ��̴��� Bounds ��������
        Bounds myBounds = GetComponent<Collider>().bounds;
        Bounds otherBounds = other.bounds;

        // ���� ���� ���
        Bounds intersection = GetIntersectionBounds(myBounds, otherBounds);

        // �����ϴ� ���� ���
        float intersectionHeight = intersection.size.y;

        Vector3 newPosition = transform.position + (Vector3.up * intersectionHeight);

        transform.position = newPosition;


        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = rb;

        photonView.RPC("RPCIntersectionBoundsMove", RpcTarget.Others, newPosition, joint, rb);

    }

    [PunRPC]
    private void RPCIntersectionBoundsMove(Vector3 newPosition, FixedJoint joint, Rigidbody rb)
    {
        Debug.Log("RPC �ٿ��� ���갡 �����");
        transform.position = newPosition;
        joint.connectedBody = rb;
    }

    [PunRPC]
    private void RPCGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
        Rigidbody grabValveRD = transform.GetComponent<Rigidbody>();
        Destroy(GetComponent<FixedJoint>());
        if (grabValveRD != null)
        {
            grabValveRD.useGravity = !grabbed;
        }
    }

    // �� Bounds�� ���� ������ ����ϴ� �Լ�
    private Bounds GetIntersectionBounds(Bounds bounds1, Bounds bounds2)
    {
        float minX = Mathf.Max(bounds1.min.x, bounds2.min.x);
        float minY = Mathf.Max(bounds1.min.y, bounds2.min.y);
        float minZ = Mathf.Max(bounds1.min.z, bounds2.min.z);

        float maxX = Mathf.Min(bounds1.max.x, bounds2.max.x);
        float maxY = Mathf.Min(bounds1.max.y, bounds2.max.y);
        float maxZ = Mathf.Min(bounds1.max.z, bounds2.max.z);

        Vector3 min = new Vector3(minX, minY, minZ);
        Vector3 max = new Vector3(maxX, maxY, maxZ);

        Bounds intersection = new Bounds();
        intersection.SetMinMax(min, max);

        return intersection;
    }

}
