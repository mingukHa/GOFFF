using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using System.Text.RegularExpressions;

public class FirebaseAccount : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Username;
    [SerializeField]
    private TMP_InputField Password;
    [SerializeField]
    private TMP_InputField passwordcheck;
    [SerializeField]
    private TextMeshProUGUI passwordMessage;
    [SerializeField]
    private TextMeshProUGUI passwordMatchMessage;
   // [SerializeField]
    //private TextMeshProUGUI feedbackMessage; // 사용자 피드백 메시지
    [SerializeField]
    private Button acountet;
    [SerializeField]
    private GameObject acountUI;

    private DatabaseReference database;

    private void Start()
    {
        // Firebase Database 초기화 확인
        if (FirebaseInitializer.Database == null)
        {
            Debug.LogError("Firebase Database 초기화 실패");
           // feedbackMessage.text = "Firebase 초기화 실패. 다시 시도하세요.";
          //  feedbackMessage.color = Color.red;
            return;
        }

        database = FirebaseInitializer.Database;

        acountet.interactable = false; // 회원가입 버튼 비활성화
        Password.onValueChanged.AddListener(OnPasswordChanged);
        passwordcheck.onValueChanged.AddListener(OnPasswordCheckChanged);
        acountet.onClick.AddListener(OnSignUpClicked);
    }

    private void FixedUpdate()
    {
        ToggleSignUpButton();
    }

    private void ToggleSignUpButton()
    {
        if (passwordMessage.color == Color.green && passwordMatchMessage.color == Color.green)
        {
            acountet.interactable = true; // 조건이 충족되면 활성화
        }
        else
        {
            acountet.interactable = false; // 조건 미충족 시 비활성화
        }
    }

    private void OnSignUpClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
           // feedbackMessage.text = "사용자 이름을 입력하세요.";
           // feedbackMessage.color = Color.red;
            return;
        }

        if (!IsPasswordMatch())
        {
            passwordMatchMessage.text = "비밀번호가 일치하지 않습니다.";
            passwordMatchMessage.color = Color.red;
            return;
        }

        if (!IsPassword(password))
        {
            passwordMessage.text = "비밀번호는 영어와 숫자로 6~10자 이내로 입력하세요.";
            passwordMessage.color = Color.red;
            return;
        }

        // 사용자 이름 중복 확인
        database.Child("users").Child(username).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("데이터베이스 접근 실패: " + task.Exception);
              //  feedbackMessage.text = "데이터베이스 접근 실패. 다시 시도하세요.";
              //  feedbackMessage.color = Color.red;
                return;
            }

            if (task.Result.Exists)
            {
                Debug.LogWarning("이미 존재하는 사용자 이름");
               // feedbackMessage.text = "이미 사용 중인 사용자 이름입니다.";
               // feedbackMessage.color = Color.red;
            }
            else
            {
                // 사용자 정보 저장
                SaveUserData(username, password);
            }
        });
    }

    private void SaveUserData(string username, string password)
    {
        User newUser = new User(username, password);
        string jsonData = JsonUtility.ToJson(newUser);

        database.Child("users").Child(username).SetRawJsonValueAsync(jsonData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("회원가입 성공!");
             //   feedbackMessage.text = "회원가입 성공!";
             //   feedbackMessage.color = Color.green;
                acountUI.SetActive(false); // UI 닫기
            }
            else
            {
                Debug.LogError("회원가입 실패: " + task.Exception);
               // feedbackMessage.text = "회원가입 실패. 다시 시도하세요.";
              //  feedbackMessage.color = Color.red;
            }
        });
    }

    private void OnPasswordChanged(string password)
    {
        if (IsPassword(password))
        {
            passwordMessage.text = "사용 가능한 비밀번호입니다.";
            passwordMessage.color = Color.green;
        }
        else
        {
            passwordMessage.text = "비밀번호는 영어와 숫자로 6~10자 이내로 입력하세요.";
            passwordMessage.color = Color.red;
        }
    }

    private void OnPasswordCheckChanged(string confirmPassword)
    {
        if (IsPasswordMatch())
        {
            passwordMatchMessage.text = "비밀번호가 일치합니다.";
            passwordMatchMessage.color = Color.green;
        }
        else
        {
            passwordMatchMessage.text = "비밀번호가 일치하지 않습니다.";
            passwordMatchMessage.color = Color.red;
        }
    }

    private bool IsPassword(string password)
    {
        if (password.Length < 6 || password.Length > 10)
        {
            return false;
        }
        Regex regex = new Regex("^[a-zA-Z0-9]+$");
        return regex.IsMatch(password);
    }

    private bool IsPasswordMatch()
    {
        return Password.text == passwordcheck.text;
    }

    [System.Serializable]
    private class User
    {
        public string username;
        public string password;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}





