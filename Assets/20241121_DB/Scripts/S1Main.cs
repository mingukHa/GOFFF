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
    //private TextMeshProUGUI feedbackMessage; // ���� �޽����� ǥ���� UI

    private DatabaseReference database;

    private void Start()
    {
        // Firebase �ʱ�ȭ Ȯ��
        if (FirebaseInitializer.Database == null)
        {
            Debug.LogError("Firebase Database�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
       //     feedbackMessage.text = "Firebase �ʱ�ȭ ����. �ٽ� �õ��ϼ���.";
       //     feedbackMessage.color = Color.red;
            return;
        }

        database = FirebaseInitializer.Database;

        // ��ư �̺�Ʈ ����
        Loginbt.onClick.AddListener(() => Login(username.text, password.text));
        joinbt.onClick.AddListener(() => OnjoinUI(true));
        exitbt.onClick.AddListener(() => OnExitUI(true));
        Jclosebt.onClick.AddListener(() => Jclose(false));
        Eclosebt.onClick.AddListener(() => Eclose(false));
    }

    // Firebase Realtime Database�� �α���
    private void Login(string username, string password)
    {
        // �Է°� ����
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
       //     feedbackMessage.text = "���̵�� ��й�ȣ�� �Է��ϼ���.";
       //     feedbackMessage.color = Color.red;
            return;
        }

        // Realtime Database���� ����� ���� ��ȸ
        database.Child("users").Child(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"�����ͺ��̽� ���� ����: {task.Exception}");
          //      feedbackMessage.text = "���� ������ �α��ο� �����߽��ϴ�.";
           //     feedbackMessage.color = Color.red;
                return;
            }

            if (task.Result.Exists)
            {
                var userData = task.Result.Value as System.Collections.Generic.Dictionary<string, object>;

                if (userData != null && userData.ContainsKey("password"))
                {
                    string storedPassword = userData["password"].ToString();

                    // ��й�ȣ Ȯ��
                    if (storedPassword == password)
                    {
                        Debug.Log("�α��� ����!");
           //             feedbackMessage.text = "�α��� ����!";
              //          feedbackMessage.color = Color.green;

                        // �α��� ���� �� �� ��ȯ
                        SceneManager.LoadScene("Scene2");
                    }
                    else
                    {
                        Debug.LogWarning("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
             //           feedbackMessage.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
              //          feedbackMessage.color = Color.red;
                    }
                }
                else
                {
                    Debug.LogError("������ ���� ����: 'password' Ű�� ã�� �� �����ϴ�.");
                //    feedbackMessage.text = "����� ������ �������� �� �����߽��ϴ�.";
                 //   feedbackMessage.color = Color.red;
                }
            }
            else
            {
                Debug.LogWarning("����ڸ� ã�� �� �����ϴ�.");
             //   feedbackMessage.text = "�������� �ʴ� ����� �̸��Դϴ�.";
             //   feedbackMessage.color = Color.red;
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





