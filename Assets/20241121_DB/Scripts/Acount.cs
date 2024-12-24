using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using System.Text.RegularExpressions;
using Firebase.Extensions;

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
    private TextMeshProUGUI passwordCheckicon;
    [SerializeField]
    private TextMeshProUGUI passwordMatcgicon;
    [SerializeField]
    private TextMeshProUGUI idCheckMessage;
    [SerializeField]
    private TextMeshProUGUI idCheckIcon;
    [SerializeField]
    private Button idck;
    [SerializeField]
    private TextMeshProUGUI idckOX;
    [SerializeField]
    private TextMeshProUGUI idckMessage;
    [SerializeField]
    private Button acountet;
    [SerializeField]
    private GameObject acountUI;

    private FirebaseAuth auth;
    private DatabaseReference database;

    private void Start()
    {
        // Firebase �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;
        database = FirebaseDatabase.DefaultInstance.RootReference;

        acountet.interactable = false; // ȸ������ ��ư ��Ȱ��ȭ
        Password.onValueChanged.AddListener(OnPasswordChanged);
        passwordcheck.onValueChanged.AddListener(OnPasswordCheckChanged);
        idck.onClick.AddListener(OnIdCheckClicked);
        acountet.onClick.AddListener(OnSignUpClicked);
    }

    private void FixedUpdate()
    {
        ToggleSignUpButton();
    }

    private void ToggleSignUpButton()
    {
        if (idckOX.color == Color.green && passwordCheckicon.color == Color.green && passwordMatcgicon.color == Color.green)
        {
            acountet.interactable = true; // ��� ������ �ʷϻ��� �� ȸ������ ��ư Ȱ��ȭ
        }
        else
        {
            acountet.interactable = false; // ������ �ϳ��� �������� ������ ��Ȱ��ȭ
        }
    }

    // Firebase Realtime Database�� ����� ���̵� �ߺ� Ȯ��
    private void OnIdCheckClicked()
    {
        string username = Username.text.Trim();
        if (string.IsNullOrEmpty(username))
        {
            idCheckMessage.text = "���̵� �Է��ϼ���.";
            idCheckMessage.color = Color.red;
            idCheckIcon.text = "X";
            idCheckIcon.color = Color.red;
            return;
        }

        // Firebase���� ���̵� �ߺ� Ȯ��
        database.Child("users").Child(username).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    idckOX.color = Color.red;
                    idckOX.text = "X";
                    idckMessage.text = "�̹� ��� ���� ���̵��Դϴ�.";
                    idckMessage.color = Color.red;
                }
                else
                {
                    idckOX.color = Color.green;
                    idckOX.text = "O";
                    idckMessage.text = "��� ������ ���̵��Դϴ�!";
                    idckMessage.color = Color.green;
                }
            }
            else
            {
                Debug.LogError("Firebase���� ������ �������� ����: " + task.Exception);
            }
        });
    }

    // Firebase Authentication�� ����� ȸ������
    private void OnSignUpClicked()
    {
        string username = Username.text.Trim();
        string password = Password.text.Trim();

        if (!IsPasswordMatch())
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            passwordMatchMessage.color = Color.red;
            return;
        }

        if (!IsPassword(password))
        {
            passwordMessage.text = "��й�ȣ�� ����, ���ڷ� 4~10�� �̳��� �Է��ϼ���.";
            passwordMessage.color = Color.red;
            return;
        }

        // Firebase Authentication���� ȸ������ ó��
        auth.CreateUserWithEmailAndPasswordAsync($"{username}", password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"ȸ������ ����: {task.Exception}");
                return;
            }

            // Firebase AuthResult�� ���� ����� ���� ��������
            Firebase.Auth.AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;

            Debug.Log($"ȸ������ ����! ����� ID: {newUser.UserId}, �̸���: {newUser.Email}");

            // ����� ������ Firebase Realtime Database�� ����
            database.Child("users").Child(username).SetValueAsync(newUser.UserId).ContinueWithOnMainThread(dbTask =>
            {
                if (dbTask.IsCompleted)
                {
                    Debug.Log("Firebase�� ����� ���� ���� ����!");
                    acountUI.SetActive(false); // UI �ݱ�
                }
                else
                {
                    Debug.LogError("Firebase�� ����� ���� ���� ����: " + dbTask.Exception);
                }
            });
        });
    }


    // ��й�ȣ ��ȿ�� �˻�
    private void OnPasswordChanged(string password)
    {
        if (IsPassword(password))
        {
            passwordMessage.text = "��� ������ ��й�ȣ�Դϴ�.";
            passwordMessage.color = Color.green;
            passwordCheckicon.text = "O";
            passwordCheckicon.color = Color.green;
        }
        else
        {
            passwordMessage.text = "��й�ȣ�� ����� ���ڷ� 4~10�� �̳��� �Է��ϼ���.";
            passwordMessage.color = Color.red;
            passwordCheckicon.text = "X";
            passwordCheckicon.color = Color.red;
        }
    }

    // ��й�ȣ ��ġ Ȯ��
    private void OnPasswordCheckChanged(string confirmPassword)
    {
        if (IsPasswordMatch())
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ�մϴ�.";
            passwordMatchMessage.color = Color.green;
            passwordMatcgicon.text = "O";
            passwordMatcgicon.color = Color.green;
        }
        else
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
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


