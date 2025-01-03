using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // 이 버튼이 나타내는 숫자
    public Transform screenDisplayPoint; // 숫자가 표시될 화면의 부모 Transform
    public GameObject numberPrefab; // 표시할 숫자 프리팹

    private KeypadManager keypadManager;

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
    }

    public void OnPressed()
    {
        Debug.Log(number +"번 버튼을 눌렀습니다.");

        // 숫자 버튼을 눌렀을 때 동작
        GameObject displayedNumber = Instantiate(numberPrefab, screenDisplayPoint);
        displayedNumber.transform.localScale = new Vector3(2, 2, 1); // x, y축으로 2배 크기 조정
        keypadManager.AddNumber(number);
    }
}
