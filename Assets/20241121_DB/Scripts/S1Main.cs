using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using Firebase.Extensions;

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
   // [SerializeField]
    //private TextMeshProUGUI feedbackMessage; // 에러 메시지를 표시할 UI

    private DatabaseReference database;

    private void Start()
    {
        // Firebase 초기화 확인
        if (FirebaseInitializer.Database == null)
        {
            Debug.LogError("Firebase Database가 초기화되지 않았습니다.");
       //     feedbackMessage.text = "Firebase 초기화 실패. 다시 시도하세요.";
       //     feedbackMessage.color = Color.red;
            return;
        }

        database = FirebaseInitializer.Database;

        // 버튼 이벤트 연결
        Loginbt.onClick.AddListener(() => Login(username.text, password.text));
        joinbt.onClick.AddListener(() => OnjoinUI(true));
        exitbt.onClick.AddListener(() => OnExitUI(true));
        Jclosebt.onClick.AddListener(() => Jclose(false));
        Eclosebt.onClick.AddListener(() => Eclose(false));
    }

    // Firebase Realtime Database로 로그인
    private void Login(string username, string password)
    {
        // 입력값 검증
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
       //     feedbackMessage.text = "아이디와 비밀번호를 입력하세요.";
       //     feedbackMessage.color = Color.red;
            return;
        }

        // Realtime Database에서 사용자 정보 조회
        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"데이터베이스 접근 실패: {task.Exception}");
          //      feedbackMessage.text = "서버 오류로 로그인에 실패했습니다.";
           //     feedbackMessage.color = Color.red;
                return;
            }

            if (task.Result.Exists)
            {
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    // 비밀번호 확인
                    if (storedPassword == password)
                    {
                        Debug.Log("로그인 성공!");
           //             feedbackMessage.text = "로그인 성공!";
              //          feedbackMessage.color = Color.green;

                        // 로그인 성공 후 씬 전환
                        SceneManager.LoadScene("Scene2");
                    }
                    else
                    {
                        Debug.LogWarning("비밀번호가 일치하지 않습니다.");
             //           feedbackMessage.text = "비밀번호가 일치하지 않습니다.";
              //          feedbackMessage.color = Color.red;
                    }
                }
                else
                {
                    Debug.LogError("데이터 형식 오류: 'password' 키를 찾을 수 없습니다.");
                //    feedbackMessage.text = "사용자 정보를 가져오는 데 실패했습니다.";
                 //   feedbackMessage.color = Color.red;
                }
            }
            else
            {
                Debug.LogWarning("사용자를 찾을 수 없습니다.");
             //   feedbackMessage.text = "존재하지 않는 사용자 이름입니다.";
             //   feedbackMessage.color = Color.red;
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





