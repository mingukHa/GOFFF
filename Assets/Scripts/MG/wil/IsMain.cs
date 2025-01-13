using UnityEngine;
using Photon.Pun;

public class PlayerControllerManager : MonoBehaviourPunCallbacks
{
    private GameObject[] CameraOffset;
    private GameObject[] handOffset;


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        CameraOffset = GameObject.FindGameObjectsWithTag("PlayerCamera");
        handOffset = GameObject.FindGameObjectsWithTag("PlayerController");
        foreach(GameObject obj in CameraOffset)
        {
            if(!obj.GetPhotonView().IsMine)
            {
                Debug.Log("���� ī�޶� ��");
                obj.SetActive(false);
            }
        }
        foreach(GameObject obj in handOffset)
        {
            if(!obj.GetPhotonView().IsMine)
            {
                Debug.Log("���� ��Ʈ�ѷ��� ��");
                obj.SetActive(false);
            }
        }
        //handOffset = transform.Find("handOffset")?.gameObject;


        //if (!photonView.IsMine)
        //{
        //    DisableControllers();
        //    //photonView.RequestOwnership();
        //    Disablehand();
        //}
        //else
        //{
        //    EnableControllers();
        //    Enablehand();
        //}
    }

    //private void Start()
    //{
    //    if (!photonView.IsMine)
    //    {
    //        // �ٸ� �÷��̾��� ��Ʈ�ѷ��� ������ �� ��ȣ�ۿ� ��Ȱ��ȭ
    //        foreach (var renderer in GetComponentsInChildren<Renderer>())
    //        {
    //            renderer.enabled = false;
    //        }

    //        foreach (var collider in GetComponentsInChildren<Collider>())
    //        {
    //            collider.enabled = false;
    //        }
    //    }
    //}

    //private void DisableControllers()
    //{
    //    if (CameraOffset != null) CameraOffset.SetActive(false);

    //    Debug.Log("��Ȱ��ȭ �߽�");
    //}

    //private void EnableControllers()
    //{
    //    if (CameraOffset != null) CameraOffset.SetActive(true);
    //    Debug.Log("Ȱ��ȭ �߽�");
    //}
    //private void Disablehand()
    //{
    //    if (CameraOffset != null) CameraOffset.SetActive(false);

    //    Debug.Log("��Ȱ��ȭ �߽�");
    //}

    //private void Enablehand()
    //{
    //    if (CameraOffset != null) CameraOffset.SetActive(true);
    //    Debug.Log("Ȱ��ȭ �߽�");
    //}
}