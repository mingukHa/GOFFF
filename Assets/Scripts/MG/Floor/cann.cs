using Photon.Pun;
using UnityEngine;

public class GrabSync : MonoBehaviourPun, IPunObservable
{
    private bool isGrabbed = false; // 오브젝트가 잡혔는지 여부
    private Transform grabber;     // 잡고 있는 플레이어의 Transform

    void Update()
    {
        // 로컬 플레이어만 상태 변경 가능
        if (photonView.IsMine && isGrabbed && grabber != null)
        {
            // 오브젝트를 grabber(손)의 위치로 이동
            transform.position = grabber.position;
            transform.rotation = grabber.rotation;
        }
    }

    // 오브젝트를 잡는 동작
    public void Grab(Transform grabberTransform)
    {
        if (photonView.IsMine)
        {
            isGrabbed = true;
            grabber = grabberTransform;

            Debug.Log("Object grabbed.");
        }
    }

    // 오브젝트를 놓는 동작
    public void Release()
    {
        if (photonView.IsMine)
        {
            isGrabbed = false;
            grabber = null;

            Debug.Log("Object released.");
        }
    }

    // Photon 동기화를 위한 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 로컬 플레이어 데이터 전송
        {
            stream.SendNext(isGrabbed);
            if (isGrabbed && grabber != null)
            {
                stream.SendNext(grabber.position);
                stream.SendNext(grabber.rotation);
            }
        }
        else // 원격 플레이어 데이터 수신
        {
            isGrabbed = (bool)stream.ReceiveNext();
            if (isGrabbed)
            {
                Vector3 grabbedPosition = (Vector3)stream.ReceiveNext();
                Quaternion grabbedRotation = (Quaternion)stream.ReceiveNext();

                // 원격 데이터 적용
                transform.position = grabbedPosition;
                transform.rotation = grabbedRotation;
            }
        }
    }
}
