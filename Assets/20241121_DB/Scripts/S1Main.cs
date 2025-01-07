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
        // Firebase 초기화 시작
        StartCoroutine(InitializeFirebase());

        // 버튼 이벤트 연결
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
            Debug.Log("랜덤 방 입장을 시도합니다.");
            PhotonNetwork.JoinRandomRoom(); // 랜덤 방에 입장 시도
        }
        else
        {
            Debug.LogFormat("Photon 연결 시작: {0}", gameVersion);
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings(); // Photon 서버에 연결
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon 서버 연결 성공. 랜덤 방 입장을 시도합니다.");
        ConnectToRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"랜덤 방 입장 실패: {message}");
        CreateRoom(); // 방이 없으면 새로운 방 생성
    }

    private void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(1000, 9999); // 랜덤한 방 이름 생성
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayerPerRoom
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions); // 방 생성
        Debug.Log($"새로운 방 생성: {roomName}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"방 생성 실패: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"방에 입장했습니다: {PhotonNetwork.CurrentRoom.Name}");
        PhotonNetwork.LoadLevel("waitRoom"); // 대기실 씬으로 이동
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase 초기화 중...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database 초기화 성공!");
            else
                Debug.LogError("Firebase Database 초기화 실패!");
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyTask.Result}");
            yield break;
        }
    }

    private void Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("아이디와 비밀번호를 입력하세요.");
            return;
        }

        if (database == null)
        {
            Debug.LogError("Firebase Database가 초기화되지 않았습니다.");
            return;
        }

        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"데이터베이스 접근 실패: {task.Exception}");
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
                        Debug.Log("로그인 성공!");

                        // Photon 닉네임 설정
                        PhotonNetwork.NickName = username;

                        // Photon 서버 연결 시도
                        if (PhotonNetwork.IsConnected)
                        {
                            Debug.Log("Photon 서버에 이미 연결됨. 랜덤 방 입장 시도...");
                            ConnectToRandomRoom();
                        }
                        else
                        {
                            Debug.Log("Photon 서버에 연결 중...");
                            PhotonNetwork.ConnectUsingSettings();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("비밀번호가 일치하지 않습니다.");
                    }
                }
                else
                {
                    Debug.LogError("데이터 형식 오류: 'password' 키를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("사용자를 찾을 수 없습니다.");
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