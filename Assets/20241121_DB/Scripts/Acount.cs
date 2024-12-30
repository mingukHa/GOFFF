using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;
using System.Text.RegularExpressions;
using System.Collections;
using Firebase.Extensions;

public class FireBaseLog : MonoBehaviour
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
    private TextMeshProUGUI passwordCheckicon;
    [SerializeField]
    private TextMeshProUGUI passwordMatcgicon;
    [SerializeField]
    private Button acountet;
    [SerializeField]
    private GameObject acountUI;

    private DatabaseReference database;

    private void Start()
    {
        StartCoroutine(InitializeFirebase());

        // 버튼 이벤트 연결
        Password.onValueChanged.AddListener(OnPasswordChanged);
        passwordcheck.onValueChanged.AddListener(OnPasswordCheckChanged);
        acountet.onClick.AddListener(OnSignUpClicked);

        acountet.interactable = false; // 회원가입 버튼 비활성화
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase Database 초기화 대기 중...");
        while (database == null)
        {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            yield return null;
        }
        Debug.Log("Firebase Database 초기화 완료!");
    }

    private void FixedUpdate()
    {
        ToggleSignUpButton();
    }

    private void ToggleSignUpButton()
    {
        // 비밀번호 체크와 비밀번호 일치 여부만 기반으로 버튼 활성화
        if (passwordCheckicon.color == Color.green && passwordMatcgicon.color == Color.green)
        {
            acountet.interactable = true;
        }
        else
        {
            acountet.interactable = false;
        }
    }

    private void OnSignUpClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        // 입력값 검증
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("아이디와 비밀번호를 입력하세요.");
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
            passwordMessage.text = "비밀번호는 영어와 숫자로 4~10자 이내로 입력하세요.";
            passwordMessage.color = Color.red;
            return;
        }

        // Firebase Database에 사용자 데이터 저장
        var userData = new System.Collections.Generic.Dictionary<string, object>
        {
            { "password", password }
        };

        database.Child("users").Child(username).SetValueAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase에 사용자 정보 저장 실패: " + task.Exception);
            }
            else
            {
                Debug.Log("회원가입 성공!");
                passwordMessage.text = "회원가입이 완료되었습니다!";
                passwordMessage.color = Color.green;
                acountUI.SetActive(false); // UI 닫기
            }
        });
    }

    // 비밀번호 유효성 검사
    private void OnPasswordChanged(string password)
    {
        if (IsPassword(password))
        {
            passwordMessage.text = "사용 가능한 비밀번호입니다.";
            passwordMessage.color = Color.green;
            passwordCheckicon.text = "O";
            passwordCheckicon.color = Color.green;
        }
        else
        {
            passwordMessage.text = "비밀번호는 영어와 숫자로 4~10자 이내로 입력하세요.";
            passwordMessage.color = Color.red;
            passwordCheckicon.text = "X";
            passwordCheckicon.color = Color.red;
        }
    }

    // 비밀번호 일치 확인
    private void OnPasswordCheckChanged(string confirmPassword)
    {
        if (IsPasswordMatch())
        {
            passwordMatchMessage.text = "비밀번호가 일치합니다.";
            passwordMatchMessage.color = Color.green;
            passwordMatcgicon.text = "O";
            passwordMatcgicon.color = Color.green;
        }
        else
        {
            passwordMatchMessage.text = "비밀번호가 일치하지 않습니다.";
            passwordMatchMessage.color = Color.red;
            passwordMatcgicon.text = "X";
            passwordMatcgicon.color = Color.red;
        }
    }

    private bool IsPassword(string password)
    {
        if (password.Length < 4 || password.Length > 10)
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
}
