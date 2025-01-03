using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // �� ��ư�� ��Ÿ���� ����
    public Transform screenDisplayPoint; // ���ڰ� ǥ�õ� ȭ���� �θ� Transform
    public GameObject numberPrefab; // ǥ���� ���� ������

    private KeypadManager keypadManager;

    private void Start()
    {
        keypadManager = FindObjectOfType<KeypadManager>();
    }

    public void OnPressed()
    {
        Debug.Log(number +"�� ��ư�� �������ϴ�.");

        // ���� ��ư�� ������ �� ����
        GameObject displayedNumber = Instantiate(numberPrefab, screenDisplayPoint);
        displayedNumber.transform.localScale = new Vector3(2, 2, 1); // x, y������ 2�� ũ�� ����
        keypadManager.AddNumber(number);
    }
}
