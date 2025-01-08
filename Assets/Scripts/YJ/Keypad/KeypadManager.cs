using UnityEngine;
using Photon.Pun;

public class KeypadManager : MonoBehaviourPun
{
    public Transform[] displayPoints; // 100�ڸ�, 10�ڸ�, 1�ڸ� ��ġ�� ���� �迭
    public DoorController doorController; // ���� ����� DoorController
    [SerializeField]
    private string correctPassword = "346"; // ���� ��й�ȣ
    private string enteredPassword = ""; // �Էµ� ��й�ȣ
    private GameObject[] displayedNumbers = new GameObject[3]; // ǥ�õ� ���� ������Ʈ
    private bool isDoorOpened = false; // ���� �̹� ���ȴ��� ���θ� üũ�ϴ� ����

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
        if (!isDoorOpened && enteredPassword == correctPassword)
        {
            doorController.OpenDoor();
            Debug.Log("�ùٸ� ��й�ȣ�Դϴ�. ���� ���Ƚ��ϴ�.");
            isDoorOpened = true;
        }
        else if (enteredPassword != correctPassword)
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
