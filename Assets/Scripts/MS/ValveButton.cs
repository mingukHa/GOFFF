using UnityEngine;
using Photon.Pun;

public class ValveButton : MonoBehaviourPun
{
    [SerializeField]
    private Valve valve;

    private bool buttonPush = false;

    private float buttonDownValue = 0f;
    private float buttonPositionY = 0f;
    private float smoothTime = 0.001f; // 부드럽게 이동하는 시간
    private float velocity = 0f; // 속도는 내부적으로 업데이트됨
    private Vector3 currentPosition;

    private void Start()
    {
        buttonPositionY = transform.localPosition.y;
        currentPosition = transform.localPosition;
    }

    private void Update()
    {
        if(buttonPush)
        {
            if (valve.knobValve.activeSelf)
            {
                Debug.Log("버튼이 눌렸음");
                buttonPositionY = Mathf.SmoothDamp(buttonPositionY, buttonDownValue, ref velocity, smoothTime);
                transform.localPosition = new Vector3(0, buttonPositionY, 0);
            }
        }

        if(buttonPositionY < 0.010)
        {
            Debug.Log("버튼이 눌리고 Y가 일정 값 이하로 떨어졌기 때문에 밸브가 떨어짐");
            valve.DetachFromCylinder();
            Debug.Log("버튼 false가 실행됨");
            buttonPush = false;
            transform.localPosition = currentPosition;
            buttonPositionY = currentPosition.y;
        }
    }

    public void SelectOnButton()
    {
        if(valve.IsAttached)
        {
            buttonPush = true;
            photonView.RPC("RPCOnButton", RpcTarget.Others, true);
        }
    }

    [PunRPC]
    private void RPCOnButton(bool button)
    {
        buttonPush = button;
    }
}
