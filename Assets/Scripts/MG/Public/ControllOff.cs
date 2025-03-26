using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class ControllerOff : MonoBehaviourPun //컨트롤러 강제 종료

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

