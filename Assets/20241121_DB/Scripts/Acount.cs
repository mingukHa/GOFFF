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
        acountet.interactable = false; // ȸ������ ��ư ��Ȱ��ȭ
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
        // ��й�ȣ ���ǰ� ��й�ȣ ��ġ ���Ǹ� �����ϸ� ȸ������ ��ư Ȱ��ȭ
        if (passwordMessage.color == Color.green && passwordMatchMessage.color == Color.green)
        {
            acountet.interactable = true; // ������ �����Ǹ� Ȱ��ȭ
        }
        else
        {
            acountet.interactable = false; // ���� ������ �� ��Ȱ��ȭ
        }
    }

    // ȸ������ ó��
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
            passwordMessage.text = "��й�ȣ�� ����, ���ڷ� 6~10�� �̳��� �Է��ϼ���.";
            passwordMessage.color = Color.red;
            return;
        }

        // Firebase Realtime Database�� ����� ���� ����
        database.Child("users").Child(username).SetRawJsonValueAsync(JsonUtility.ToJson(new User(username, password))).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("ȸ������ ����!");
                acountUI.SetActive(false); // UI �ݱ�
            }
            else
            {
                Debug.LogError("ȸ������ ����: " + task.Exception);
            }
        });
    }

    private void OnPasswordChanged(string password)
    {
        if (IsPassword(password))
        {
            passwordMessage.text = "��� ������ ��й�ȣ�Դϴ�.";
            passwordMessage.color = Color.green;
        }
        else
        {
            passwordMessage.text = "��й�ȣ�� ����� ���ڷ� 6~10�� �̳��� �Է��ϼ���.";
            passwordMessage.color = Color.red;
        }
    }

    private void OnPasswordCheckChanged(string confirmPassword)
    {
        if (IsPasswordMatch())
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ�մϴ�.";
            passwordMatchMessage.color = Color.green;
        }
        else
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
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




