using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class ControllerOff : MonoBehaviourPun //��Ʈ�ѷ� ���� ����

{
    public GameObject handOffset;
    public GameObject Cameras;
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            handOffset.SetActive(false);
            Cameras.SetActive(false);
        }
    }
}

