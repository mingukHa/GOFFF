using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

public class JoinEchoChannel : MonoBehaviour
{

    public event Action OnLoginEndEvent;

    public static JoinEchoChannel Instance { get; private set; }

    //private VivoxParticipant Participant;

    //private bool isMuted = true;
    private void Awake()
    {

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private async void Start()
    {

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();

        await LoginAsync();

        OnLoginEndEvent?.Invoke();

    }

    private async Task LoginAsync()
    {

        LoginOptions options = new LoginOptions();
        options.DisplayName = Guid.NewGuid().ToString();

        await VivoxService.Instance.LoginAsync(options);

    }

    public async void JoinVoiceChannel(string channelName)
    {

        //음성채팅 채널에 접속
        await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.AudioOnly);
        //await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.AudioOnly);
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        // A 키를 누르고 있을 때만 언뮤트
    //        if (isMuted)
    //        {
    //            Participant.UnmutePlayerLocally();
    //            isMuted = false; // 언뮤트 상태로 변경
    //        }
    //    }
    //    else
    //    {
    //        // A 키를 떼면 뮤트 상태로 되돌리기
    //        if (!isMuted)
    //        {
    //            Participant.MutePlayerLocally();
    //            isMuted = true; // 뮤트 상태로 변경
    //        }
    //    }
    //}

}