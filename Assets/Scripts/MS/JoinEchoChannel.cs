using Photon.Pun;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.XR;

public class JoinEchoChannel : MonoBehaviourPun
{

    public event Action OnLoginEndEvent;

    public static JoinEchoChannel Instance { get; private set; }

    private string channelName = "Channel";

    private bool isMuted = true;

    private bool previousButtonState = false;



    //private VivoxParticipant Participant;

    //private bool isMuted = true;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private async void Start()
    {
        if (!photonView.IsMine) return;
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();

        await LoginAsync();

        await JoinVoiceChannel(channelName);
        Debug.Log("보이스 채널 접속이 됐습니다.");

        VivoxService.Instance.MuteOutputDevice();


    }

    private async Task LoginAsync()
    {

        LoginOptions options = new LoginOptions();
        options.DisplayName = Guid.NewGuid().ToString();

        await VivoxService.Instance.LoginAsync(options);

    }

    public async Task JoinVoiceChannel(string channelName)
    {

        //음성채팅 채널에 접속
        //await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.AudioOnly);
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.AudioOnly);
    }

    private void Update()
    {
        // 현재 상태 가져오기
        InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool currentButtonState))
        {
            //Debug.Log("채팅 참여 키 누르고 있음");
            // 버튼 상태가 이전 프레임과 동일하면 처리하지 않음
            if (currentButtonState != previousButtonState)
            {
                if (!photonView.IsMine)
                {
                    photonView.RequestOwnership();
                }
                if (currentButtonState) // 버튼이 눌렸을 때
                {
                    if (isMuted)
                    {
                        VivoxService.Instance.UnmuteOutputDevice();
                        isMuted = false; // 언뮤트 상태로 변경
                    }
                }
                else // 버튼이 떼졌을 때
                {
                    if (!isMuted)
                    {
                        VivoxService.Instance.MuteOutputDevice();
                        isMuted = true; // 뮤트 상태로 변경
                    }
                }
            }

            // 버튼 상태 업데이트
            previousButtonState = currentButtonState;
        }
    }
}