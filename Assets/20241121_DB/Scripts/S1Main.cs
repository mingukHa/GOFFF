using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Firebase.Auth;
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

    private FirebaseAuth auth;

    private void Start()
    {
        // Firebase Authentication �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;

        // ��ư �̺�Ʈ ����
        Loginbt.onClick.AddListener(() => Login(username.text, password.text));
        joinbt.onClick.AddListener(() => OnjoinUI(true));
        exitbt.onClick.AddListener(() => OnExitUI(true));
        Jclosebt.onClick.AddListener(() => Jclose(false));
        Eclosebt.onClick.AddListener(() => Eclose(false));
    }
    
    // Firebase�� �α���
    private void Login(string email, string password)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�α����� ��ҵǾ����ϴ�.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError($"�α��� ����: {task.Exception}");
                return;
            }

            // AuthResult���� FirebaseUser ��������
            Firebase.Auth.AuthResult authResult = task.Result;
            FirebaseUser user = authResult.User;

            Debug.Log($"�α��� ����! ����� ID: {user.UserId}, �̸���: {user.Email}");
            SceneManager.LoadScene("Scene2"); // �α��� ���� �� ���� ������ ��ȯ
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


