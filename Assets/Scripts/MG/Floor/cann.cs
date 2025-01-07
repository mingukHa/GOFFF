using Photon.Pun;
using UnityEngine;

public class GrabSync : MonoBehaviourPun, IPunObservable
{
    private bool isGrabbed = false; // ������Ʈ�� �������� ����
    private Transform grabber;     // ��� �ִ� �÷��̾��� Transform

    void Update()
    {
        // ���� �÷��̾ ���� ���� ����
        if (photonView.IsMine && isGrabbed && grabber != null)
        {
            // ������Ʈ�� grabber(��)�� ��ġ�� �̵�
            transform.position = grabber.position;
            transform.rotation = grabber.rotation;
        }
    }

    // ������Ʈ�� ��� ����
    public void Grab(Transform grabberTransform)
    {
        if (photonView.IsMine)
        {
            isGrabbed = true;
            grabber = grabberTransform;

            Debug.Log("Object grabbed.");
        }
    }

    // ������Ʈ�� ���� ����
    public void Release()
    {
        if (photonView.IsMine)
        {
            isGrabbed = false;
            grabber = null;

            Debug.Log("Object released.");
        }
    }

    // Photon ����ȭ�� ���� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // ���� �÷��̾� ������ ����
        {
            stream.SendNext(isGrabbed);
            if (isGrabbed && grabber != null)
            {
                stream.SendNext(grabber.position);
                stream.SendNext(grabber.rotation);
            }
        }
        else // ���� �÷��̾� ������ ����
        {
            isGrabbed = (bool)stream.ReceiveNext();
            if (isGrabbed)
            {
                Vector3 grabbedPosition = (Vector3)stream.ReceiveNext();
                Quaternion grabbedRotation = (Quaternion)stream.ReceiveNext();

                // ���� ������ ����
                transform.position = grabbedPosition;
                transform.rotation = grabbedRotation;
            }
        }
    }
}
