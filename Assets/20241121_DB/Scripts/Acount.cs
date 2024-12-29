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

        // ��ư �̺�Ʈ ����
        Password.onValueChanged.AddListener(OnPasswordChanged);
        passwordcheck.onValueChanged.AddListener(OnPasswordCheckChanged);
        acountet.onClick.AddListener(OnSignUpClicked);

        acountet.interactable = false; // ȸ������ ��ư ��Ȱ��ȭ
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase Database �ʱ�ȭ ��� ��...");
        while (database == null)
        {
            database = FirebaseDatabase.DefaultInstance.RootReference;
            yield return null;
        }
        Debug.Log("Firebase Database �ʱ�ȭ �Ϸ�!");
    }

    private void FixedUpdate()
    {
        ToggleSignUpButton();
    }

    private void ToggleSignUpButton()
    {
        // ��й�ȣ üũ�� ��й�ȣ ��ġ ���θ� ������� ��ư Ȱ��ȭ
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

        // �Է°� ����
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("���̵�� ��й�ȣ�� �Է��ϼ���.");
            return;
        }

        if (!IsPasswordMatch())
        {
            passwordMatchMessage.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            passwordMatchMessage.color = Color.red;
            return;
        }

        if (!IsPassword(password))
        {
            passwordMessage.text = "��й�ȣ�� ����� ���ڷ� 4~10�� �̳��� �Է��ϼ���.";
            passwordMessage.color = Color.red;
            return;
        }

        // Firebase Database�� ����� ������ ����
        var userData = new System.Collections.Generic.Dictionary<string, object>
        {
            { "password", password }
        };

        database.Child("users").Child(username).SetValueAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase�� ����� ���� ���� ����: " + task.Exception);
            }
            else
            {
                Debug.Log("ȸ������ ����!");
                passwordMessage.text = "ȸ�������� �Ϸ�Ǿ����ϴ�!";
                passwordMessage.color = Color.green;
                acountUI.SetActive(false); // UI �ݱ�
            }
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
