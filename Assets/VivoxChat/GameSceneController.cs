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
            // A 키를 누르고 있을 때만 언뮤트
            if (isMuted)
            {
                VivoxService.Instance.UnmuteOutputDevice();
                isMuted = false; // 언뮤트 상태로 변경
            }
        }
        else
        {
            // A 키를 떼면 뮤트 상태로 되돌리기
            if (!isMuted)
            {
                VivoxService.Instance.MuteOutputDevice();
                isMuted = true; // 뮤트 상태로 변경
            }
        }
    }

}