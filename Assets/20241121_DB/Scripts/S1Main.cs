using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;

public class S1Main : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button Jclosebt = null;
    [SerializeField]
    private Button Loginbt = null;
    [SerializeField]
    private Button Eclosebt = null;
    [SerializeField]
    private Button joinbt = null;
    [SerializeField]
    private Button exitbt = null;
    [SerializeField]
    private GameObject joinUI = null;
    [SerializeField]
    private GameObject exitUI = null;
    [SerializeField]
    private TMP_InputField username;
    [SerializeField]
    private TMP_InputField password;

    public static DatabaseReference database;

    [SerializeField]
    private string gameVersion = "0.0.1";
    [SerializeField]
    private byte maxPlayerPerRoom = 2;

    private void Start()
    {
        // Firebase �ʱ�ȭ ����
        StartCoroutine(InitializeFirebase());

        // ��ư �̺�Ʈ ����
        Loginbt.onClick.AddListener(() => Login(username.text, password.text));
        joinbt.onClick.AddListener(() => OnjoinUI(true));
        exitbt.onClick.AddListener(() => OnExitUI(true));
        Jclosebt.onClick.AddListener(() => Jclose(false));
        Eclosebt.onClick.AddListener(() => Eclose(false));
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnectToRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("���� �� ������ �õ��մϴ�.");
            PhotonNetwork.JoinRandomRoom(); // ���� �濡 ���� �õ�
        }
        else
        {
            Debug.LogFormat("Photon ���� ����: {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings(); // Photon ������ ����
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon ���� ���� ����. ���� �� ������ �õ��մϴ�.");
        ConnectToRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"���� �� ���� ����: {message}");
        CreateRoom(); // ���� ������ ���ο� �� ����
    }

    private void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(1000, 9999); // ������ �� �̸� ����
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayerPerRoom
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions); // �� ����
        Debug.Log($"���ο� �� ����: {roomName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"�� ���� ����: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"�濡 �����߽��ϴ�: {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LoadLevel("waitRoom"); // ���� ������ �̵�
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase �ʱ�ȭ ��...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase �ʱ�ȭ ����!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database �ʱ�ȭ ����!");
            else
                Debug.LogError("Firebase Database �ʱ�ȭ ����!");
        }
        else
        {
            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyTask.Result}");
            yield break;
        }
    }

    private void Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("���̵�� ��й�ȣ�� �Է��ϼ���.");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Firebase Database�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"�����ͺ��̽� ���� ����: {task.Exception}");
                return;
            }

            if (task.Result.Exists)
            {
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    if (storedPassword == password)
                    {
                        Debug.Log("�α��� ����!");

                        // Photon �г��� ����
                        PhotonNetwork.NickName = username;

                        // Photon ���� ���� �õ�
                        if (PhotonNetwork.IsConnected)
                        {
                            Debug.Log("Photon ������ �̹� �����. ���� �� ���� �õ�...");
                            ConnectToRandomRoom();
                        }
                        else
                        {
                            Debug.Log("Photon ������ ���� ��...");
                            PhotonNetwork.ConnectUsingSettings();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
                    }
                }
                else
                {
                    Debug.LogError("������ ���� ����: 'password' Ű�� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogWarning("����ڸ� ã�� �� �����ϴ�.");
            }
        });
    }

    private void Jclose(bool close)
    {
        joinUI.SetActive(close);
    }

    private void Eclose(bool close)
    {
        exitUI.SetActive(close);
    }

    private void OnExitUI(bool exit)
    {
        exitUI.SetActive(exit);
    }

    private void OnjoinUI(bool join)
    {
        joinUI.SetActive(join);
    }
}