using UnityEngine;
using Unity.Services.Vivox;
using Unity.Services.Authentication;

public class GameSceneController : MonoBehaviour
{

    [SerializeField] private string channelName;
    private VivoxParticipant Participant;
    private bool isMuted = true;

    private void Awake()
    {
        //JoinEchoChannel.Instance.JoinVoiceChannel(channelName);
    }

    private void Start()
    {
        VivoxService.Instance.MuteOutputDevice();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            // A Ű�� ������ ���� ���� ���Ʈ
            if (isMuted)
            {
                VivoxService.Instance.UnmuteOutputDevice();
                isMuted = false; // ���Ʈ ���·� ����
            }
        }
        else
        {
            // A Ű�� ���� ��Ʈ ���·� �ǵ�����
            if (!isMuted)
            {
                VivoxService.Instance.MuteOutputDevice();
                isMuted = true; // ��Ʈ ���·� ����
            }
        }
    }

}