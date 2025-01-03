using UnityEngine;

public class KeypadManager : MonoBehaviour
{
    public Transform[] displayPoints; // 100�ڸ�, 10�ڸ�, 1�ڸ� ��ġ�� ���� �迭
    public GameObject doorObject; // Door Object
    public string correctPassword = "346"; // ���� ��й�ȣ
    public DoorController dc;
    private string enteredPassword = ""; // �Էµ� ��й�ȣ
    private GameObject[] displayedNumbers = new GameObject[3]; // ǥ�õ� ���� ������Ʈ

    public void AddNumber(int number)
    {
        if (enteredPassword.Length < 3)
        {
            // �Էµ� ���� �߰�
            enteredPassword += number.ToString();

            // �ش� ���ڸ� �ùٸ� �ڸ��� ��ġ�� ǥ��
            int index = enteredPassword.Length - 1;
            GameObject numberPrefab = Resources.Load<GameObject>($"NumberPrefabs/{number}");
            displayedNumbers[index] = Instantiate(numberPrefab, displayPoints[index]);
            displayedNumbers[index].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnEnterButtonPressed()
    {
        // ǥ�õ� ���� ������Ʈ ����
        foreach (var displayedNumber in displayedNumbers)
        {
            if (displayedNumber != null)
                Destroy(displayedNumber);
        }

        // �Էµ� ��й�ȣ ����
        if (enteredPassword == correctPassword)
        {
            dc.isOpen = true;
            //doorObject.SetActive(false); // �� ��Ȱ��ȭ
            Debug.Log("Correct Password! Door is disabled.");
        }
        else
        {
            Debug.Log("Incorrect Password!");
        }

        // ���� �ʱ�ȭ
        enteredPassword = "";
        for (int i = 0; i < displayedNumbers.Length; i++)
        {
            displayedNumbers[i] = null;
        }
    }
}
