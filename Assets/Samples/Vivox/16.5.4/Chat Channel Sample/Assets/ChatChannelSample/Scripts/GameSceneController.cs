using UnityEngine;

public class GameSceneController : MonoBehaviour
{

    [SerializeField] private string channelName;

    private void Awake()
    {

        VivoxController.Instance.JoinVoiceChannel("channelName");

    }

}