using UnityEngine;
using Photon.Pun;

public class KeypadManager : MonoBehaviourPun
{
    public Transform[] displayPoints; // 100�ڸ�, 10�ڸ�, 1�ڸ� ��ġ�� ���� �迭
    public DoorManager doorController; // ���� ����� DoorController
    [SerializeField]
    private string correctPassword = "346"; // ���� ��й�ȣ
    private string enteredPassword = ""; // �Էµ� ��й�ȣ
    private GameObject[] displayedNumbers = new GameObject[3]; // ǥ�õ� ���� ������Ʈ
    private bool isDoorOpened = false; // ���� ���ȴ��� ���θ� üũ�ϴ� ����

    // ����Ű ������ �� �۵��ϴ� �޼ҵ�
    [PunRPC]
    public void AddNumber(int number)
    {
        if (enteredPassword.Length < 3)
        {
            // ���� ����Ű ��ȣ�� �Էµ� ��й�ȣ ���ڿ��� �߰�
            enteredPassword += number.ToString();

            // �ش� ���ڸ� �ùٸ� �ڸ��� ��ġ�� ǥ��
            int idx = enteredPassword.Length - 1;
            GameObject numberPrefab = Resources.Load<GameObject>($"NumberPrefabs/{number}"); // ���÷��̿� ��� ���� ������ �ε�
            displayedNumbers[idx] = Instantiate(numberPrefab, displayPoints[idx]); // ���÷��̿� ��� ���� ������Ʈ ����
            displayedNumbers[idx].transform.localScale = new Vector3(1, 1, 1); // ���� ������ normalize
        }
    }

    // ����Ű ������ �� �۵��ϴ� �޼ҵ�
    [PunRPC]
    public void OnEnterButtonPressed()
    {
        // ���� ������ ���� ���¿��� �Է��� ��й�ȣ�� �ùٸ� ��й�ȣ�� ��
        if (!isDoorOpened && enteredPassword == correctPassword)
        {
            SoundManager.instance.SFXPlay("PWCorrect_SFX", this.gameObject);

            // ���� ������ �� ���� ���¸� ������ �ݿ�
            doorController.OpenDoor();
            isDoorOpened = true;
        }
        // Ʋ�� ��й�ȣ�� ��
        else if (enteredPassword != correctPassword)
        {
            SoundManager.instance.SFXPlay("PWIncorrect_SFX", this.gameObject);
        }

        // ���÷��̿� ǥ�õ� ���� ������Ʈ�� ����
        foreach (var displayedNumber in displayedNumbers)
        {
            if (displayedNumber != null)
                Destroy(displayedNumber);
        }

        // �Էµ� ��й�ȣ�� �ʱ�ȭ
        enteredPassword = ""; 
        for (int i = 0; i < displayedNumbers.Length; ++i)
        {
            displayedNumbers[i] = null;
        }
    }
}
