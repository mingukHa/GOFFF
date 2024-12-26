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
                Debug.Log("Firebase 초기화 성공!");
                Auth = FirebaseAuth.DefaultInstance;
                Database = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {task.Result}");
                // 초기화 실패 처리 로직
                ShowInitializationError();
            }
        });
    }

    private void ShowInitializationError()
    {
        // 사용자에게 Firebase 초기화 실패 알림
        Debug.LogError("Firebase 초기화에 실패했습니다. 앱을 다시 시작하거나 네트워크를 확인하세요.");
    }

}

