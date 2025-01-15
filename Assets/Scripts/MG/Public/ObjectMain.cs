using Photon.Pun;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectMain : MonoBehaviourPun
{
    private bool isGrabbed = false; //그랩 상태

    public void OnSelectEnter() //집어지면
    {
        if(!photonView.IsMine) //내꺼가 아니면
        {
            photonView.RequestOwnership(); //권한을 넘겨받음
        }
        isGrabbed = true;
        Destroy(GetComponent<FixedJoint>());
        photonView.RPC("RPCGrabbed", RpcTarget.Others, true);
        Debug.Log("오브젝트를 집었음");
    }
    public void OnSelectFalse()
    {
        Debug.Log("물체를 놓았습니다.");
        isGrabbed = false;
        photonView.RPC("RPCGrabbed", RpcTarget.Others, false);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 발동");
        //if (!photonView.IsMine)
        //{
        //    photonView.RequestOwnership();
        //}
        if (!photonView.IsMine)
        {
            Debug.Log("내 물체가 아니라서 트리거가 아니라서 작동안함");
        }
            
        if(!isGrabbed && gameObject.GetComponent<FixedJoint>() == null)
        {
            if (other.CompareTag("Basket"))
            {
                IntersectionBoundsMove(other);
            }
        }
        Debug.Log("오브젝트가 닿았음");
    }

    private void IntersectionBoundsMove(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // 두 콜라이더의 Bounds 가져오기
        Bounds myBounds = GetComponent<Collider>().bounds;
        Bounds otherBounds = other.bounds;

        // 교차 영역 계산
        Bounds intersection = GetIntersectionBounds(myBounds, otherBounds);

        // 교차하는 높이 계산
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
        Debug.Log("RPC 바운즈 무브가 실행됨");
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

    // 두 Bounds의 교차 영역을 계산하는 함수
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
