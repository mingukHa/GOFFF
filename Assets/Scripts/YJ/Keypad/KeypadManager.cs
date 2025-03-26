using UnityEngine;
using Photon.Pun;

public class KeypadManager : MonoBehaviourPun
{
    public Transform[] displayPoints; // 100자리, 10자리, 1자리 위치를 담은 배열
    public DoorManager doorController; // 문에 연결된 DoorController
    [SerializeField]
    private string correctPassword = "346"; // 정답 비밀번호
    private string enteredPassword = ""; // 입력된 비밀번호
    private GameObject[] displayedNumbers = new GameObject[3]; // 표시된 숫자 오브젝트
    private bool isDoorOpened = false; // 문이 열렸는지 여부를 체크하는 변수

    // 숫자키 눌렀을 때 작동하는 메소드
    [PunRPC]
    public void AddNumber(int number)
    {
        if (enteredPassword.Length < 3)
        {
            // 누른 숫자키 번호를 입력된 비밀번호 문자열에 추가
            enteredPassword += number.ToString();

            // 해당 숫자를 올바른 자리수 위치에 표시
            int idx = enteredPassword.Length - 1;
            GameObject numberPrefab = Resources.Load<GameObject>($"NumberPrefabs/{number}"); // 디스플레이에 띄울 숫자 프리팹 로드
            displayedNumbers[idx] = Instantiate(numberPrefab, displayPoints[idx]); // 디스플레이에 띄울 숫자 오브젝트 생성
            displayedNumbers[idx].transform.localScale = new Vector3(1, 1, 1); // 로컬 스케일 normalize
        }
    }

    // 엔터키 눌렀을 때 작동하는 메소드
    [PunRPC]
    public void OnEnterButtonPressed()
    {
        // 문이 열리지 않은 상태에서 입력한 비밀번호가 올바른 비밀번호일 때
        if (!isDoorOpened && enteredPassword == correctPassword)
        {
            SoundManager.instance.SFXPlay("PWCorrect_SFX", this.gameObject);

            // 문을 개방한 뒤 열린 상태를 변수에 반영
            doorController.OpenDoor();
            isDoorOpened = true;
        }
        // 틀린 비밀번호일 때
        else if (enteredPassword != correctPassword)
        {
            SoundManager.instance.SFXPlay("PWIncorrect_SFX", this.gameObject);
        }

        // 디스플레이에 표시된 숫자 오브젝트를 제거
        foreach (var displayedNumber in displayedNumbers)
        {
            if (displayedNumber != null)
                Destroy(displayedNumber);
        }

        // 입력된 비밀번호를 초기화
        enteredPassword = ""; 
        for (int i = 0; i < displayedNumbers.Length; ++i)
        {
            displayedNumbers[i] = null;
        }
    }
}
