using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField Username;
    [SerializeField]
    private TMP_InputField Password;
    [SerializeField]
    private Button exitbt;
    [SerializeField]
    private GameObject exitUI;


    // Firebase Realtime Database ����
    private DatabaseReference database;

    private void Start()
    {
        // ��ư Ŭ�� �� ���� ���� �ڷ�ƾ ����
        exitbt.onClick.AddListener(() => DeleteAccount(Username.text.Trim(), Password.text.Trim()));
    }

    private void DeleteAccount(string username, string password)
    {
        Debug.Log($"���� ���� �õ�: {username}, {password}");

        // Firebase Realtime Database���� ����� ������ �˻�
        database.Child("users").OrderByChild("username").EqualTo(username).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"���� ���� ����: {task.Exception}");
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                foreach (var user in snapshot.Children)
                {
                    var userData = user.Value as System.Collections.Generic.Dictionary<string, object>;

                    if (userData != null && userData.ContainsKey("password"))
                    {
                        string storedPassword = userData["password"].ToString();

                        if (storedPassword == password)
                        {
                            // ����� ������ ����
                            database.Child("users").Child(user.Key).RemoveValueAsync().ContinueWithOnMainThread(deleteTask =>
                            {
                                if (deleteTask.IsFaulted || deleteTask.IsCanceled)
                                {
                                    Debug.LogError($"���� ���� �� ���� �߻�: {deleteTask.Exception}");
                                }
                                else
                                {
                                    Debug.Log("���� ���� �Ϸ�");
                                    exitUI.SetActive(false); // UI �ݱ�
                                }
                            });
                            return;
                        }
                        else
                        {
                            Debug.LogError("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
                            return;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("�ش� ���̵� ã�� �� �����ϴ�.");
            }
        });
    }
}
