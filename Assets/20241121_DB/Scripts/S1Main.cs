using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using System.Collections;
public class S1Main : MonoBehaviour
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

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase 초기화 중...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            // Database 초기화 성공 로그
            if (database != null)
                Debug.Log("Firebase Database 초기화 성공!");
            else
                Debug.LogError("Firebase Database 초기화 실패!");
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyTask.Result}");
            yield break; // 초기화 실패 시 종료
        }
    }


    private void Login(string username, string password)
    {
        // 입력값 검증
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("아이디와 비밀번호를 입력하세요.");
            return;
        }

        // Firebase Database 초기화 확인
        if (database == null)
        {
            Debug.LogError("Firebase Database가 초기화되지 않았습니다.");
            return;
        }

        // Realtime Database에서 사용자 정보 조회
        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"데이터베이스 접근 실패: {task.Exception}");
                return;
            }

            if (task.Result.Exists)
            {
                // 사용자 데이터를 가져오기
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    // 비밀번호 확인
                    if (storedPassword == password)
                    {
                        Debug.Log("로그인 성공!");

                        // 로그인 성공 후 씬 전환
                        SceneManager.LoadScene("Scene2");
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

    // 닫기 버튼 처리
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
