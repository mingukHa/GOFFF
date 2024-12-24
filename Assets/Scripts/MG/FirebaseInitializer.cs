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
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase ��� �غ� �Ϸ�
                Debug.Log("Firebase �ʱ�ȭ ����!");

                // Firebase ���� �ν��Ͻ� ����
                Auth = FirebaseAuth.DefaultInstance;
                Database = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                // Firebase �ʱ�ȭ ����
                Debug.LogError($"Firebase �ʱ�ȭ ����: {task.Result}");
            }
        });
    }
}

