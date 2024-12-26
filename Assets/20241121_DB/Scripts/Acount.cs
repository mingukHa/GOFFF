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
    [SerializeField]
    private Button acountet;
    [SerializeField]
    private GameObject acountUI;

    private DatabaseReference database;

    private void Start()
    {
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
        // 비밀번호 조건과 비밀번호 일치 조건만 충족하면 회원가입 버튼 활성화
        if (passwordMessage.color == Color.green && passwordMatchMessage.color == Color.green)
        {
            acountet.interactable = true; // 조건이 충족되면 활성화
        }
        else
        {
            acountet.interactable = false; // 조건 미충족 시 비활성화
        }
    }

    // 회원가입 처리
    private void OnSignUpClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (!IsPasswordMatch())
        {
            passwordMatchMessage.text = "비밀번호가 일치하지 않습니다.";
            passwordMatchMessage.color = Color.red;
            return;
        }

        if (!IsPassword(password))
        {
            passwordMessage.text = "비밀번호는 영어, 숫자로 6~10자 이내로 입력하세요.";
            passwordMessage.color = Color.red;
            return;
        }

        // Firebase Realtime Database에 사용자 정보 저장
        database.Child("users").Child(username).SetRawJsonValueAsync(JsonUtility.ToJson(new User(username, password))).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("회원가입 성공!");
                acountUI.SetActive(false); // UI 닫기
            }
            else
            {
                Debug.LogError("회원가입 실패: " + task.Exception);
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




