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

    private DatabaseReference database;

    private void Start()
    {
        // ��ư �̺�Ʈ ����
        Loginbt.onClick.AddListener(() => Login(username.text, password.text));
        joinbt.onClick.AddListener(() => OnjoinUI(true));
        exitbt.onClick.AddListener(() => OnExitUI(true));
        Jclosebt.onClick.AddListener(() => Jclose(false));
        Eclosebt.onClick.AddListener(() => Eclose(false));
    }

    private void Login(string username, string password)
    {
        // �Է°� ����
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("���̵�� ��й�ȣ�� �Է��ϼ���.");
            return;
        }

        // Realtime Database���� ����� ���� ��ȸ
        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"�����ͺ��̽� ���� ����: {task.Exception}");
                return;
            }

            if (task.Result.Exists)
            {
                // ����� �����͸� ��������
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    // ��й�ȣ Ȯ��
                    if (storedPassword == password)
                    {
                        Debug.Log("�α��� ����!");

                        // �α��� ���� �� �� ��ȯ
                        SceneManager.LoadScene("Scene2");
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

    // �ݱ� ��ư ó��
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






