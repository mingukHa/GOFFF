using UnityEngine;
using Photon.Pun;

public class KeypadManager : MonoBehaviourPun
{
    public Transform[] displayPoints; // 100자리, 10자리, 1자리 위치를 담은 배열
    public DoorController doorController; // 문에 연결된 DoorController
    [SerializeField]
    private string correctPassword = "346"; // 정답 비밀번호
    private string enteredPassword = ""; // 입력된 비밀번호
    private GameObject[] displayedNumbers = new GameObject[3]; // 표시된 숫자 오브젝트
    private bool isDoorOpened = false; // 문이 이미 열렸는지 여부를 체크하는 변수

    [PunRPC]
    public void AddNumber(int number)
    {
        if (enteredPassword.Length < 3)
        {
            // 입력된 숫자 추가
            enteredPassword += number.ToString();

            // 해당 숫자를 올바른 자리수 위치에 표시
            int index = enteredPassword.Length - 1;
            GameObject numberPrefab = Resources.Load<GameObject>($"NumberPrefabs/{number}");
            displayedNumbers[index] = Instantiate(numberPrefab, displayPoints[index]);
            displayedNumbers[index].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    [PunRPC]
    public void OnEnterButtonPressed()
    {
        // 표시된 숫자 오브젝트 제거
        foreach (var displayedNumber in displayedNumbers)
        {
            if (displayedNumber != null)
                Destroy(displayedNumber);
        }

        // 입력된 비밀번호 검증
        if (!isDoorOpened && enteredPassword == correctPassword)
        {
            doorController.OpenDoor();
            Debug.Log("올바른 비밀번호입니다. 문이 열렸습니다.");
            isDoorOpened = true;
        }
        else if (enteredPassword != correctPassword)
        {
            Debug.Log("잘못된 비밀번호입니다.");
        }

        // 상태 초기화
        enteredPassword = "";
        for (int i = 0; i < displayedNumbers.Length; i++)
        {
            displayedNumbers[i] = null;
        }
    }
}
