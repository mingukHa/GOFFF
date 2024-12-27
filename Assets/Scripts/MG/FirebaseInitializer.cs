using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseAuth Auth { get; private set; } // Firebase Authentication
    public static DatabaseReference Database { get; private set; } // Firebase Realtime Database

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase �ʱ�ȭ ����!");
                Auth = FirebaseAuth.DefaultInstance;
                Database = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Firebase �ʱ�ȭ ����: {task.Result}");
                // �ʱ�ȭ ���� ó�� ����
                ShowInitializationError();
            }
        });
    }

    private void ShowInitializationError()
    {
        // ����ڿ��� Firebase �ʱ�ȭ ���� �˸�
        Debug.LogError("Firebase �ʱ�ȭ�� �����߽��ϴ�. ���� �ٽ� �����ϰų� ��Ʈ��ũ�� Ȯ���ϼ���.");
    }

}

