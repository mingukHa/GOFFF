using UnityEngine;
using Photon.Pun;

public class ValveButton : MonoBehaviourPun
{
    [SerializeField]
    private Valve valve;

    private bool buttonPush = false;

    private float buttonDownValue = 0f;
    private float buttonPositionY = 0f;
    private float smoothTime = 0.001f; // �ε巴�� �̵��ϴ� �ð�
    private float velocity = 0f; // �ӵ��� ���������� ������Ʈ��
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
                Debug.Log("��ư�� ������");
                buttonPositionY = Mathf.SmoothDamp(buttonPositionY, buttonDownValue, ref velocity, smoothTime);
                transform.localPosition = new Vector3(0, buttonPositionY, 0);
            }
        }

        if(buttonPositionY < 0.010)
        {
            Debug.Log("��ư�� ������ Y�� ���� �� ���Ϸ� �������� ������ ��갡 ������");
            valve.DetachFromCylinder();
            Debug.Log("��ư false�� �����");
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
