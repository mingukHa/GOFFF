using UnityEngine;
using Photon.Pun;

public class KeypadManager : MonoBehaviourPun
{
    public Transform[] displayPoints; // 100�ڸ�, 10�ڸ�, 1�ڸ� ��ġ�� ���� �迭
    public GameObject doorObject; // Door Object
    public string correctPassword = "346"; // ���� ��й�ȣ
    public DoorController dc;
    private string enteredPassword = ""; // �Էµ� ��й�ȣ
    private GameObject[] displayedNumbers = new GameObject[3]; // ǥ�õ� ���� ������Ʈ

    [PunRPC]
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

    [PunRPC]
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
            Debug.Log("�ùٸ� ��й�ȣ�Դϴ�. ���� ���Ƚ��ϴ�.");
        }
        else
        {
            Debug.Log("�߸��� ��й�ȣ�Դϴ�.");
        }

        // ���� �ʱ�ȭ
        enteredPassword = "";
        for (int i = 0; i < displayedNumbers.Length; i++)
        {
            displayedNumbers[i] = null;
        }
    }
}
