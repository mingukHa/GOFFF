using System.Collections.Generic;
using UnityEngine;

public class PasswordPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] displaySlots; // ��й�ȣ�� ǥ���� ���� (���ʺ��� 3�ڸ�)
    [SerializeField] private string correctPassword = "123"; // ���� ��й�ȣ
    private List<int> inputPassword = new List<int>(); // ���� �Էµ� ��й�ȣ

    private void Start()
    {
        ClearPassword();
    }

    // ��й�ȣ �Է�
    public void EnterNumber(int number)
    {
        if (inputPassword.Count < 3) // �ִ� 3�ڸ������� �Է� ����
        {
            inputPassword.Add(number);

            // �Էµ� ��ȣ�� ǥ��
            UpdateDisplay();
        }
    }

    // �Է� �Ϸ� (Enter ��ư)
    public void CheckPassword()
    {
        string currentPassword = string.Join("", inputPassword);

        if (currentPassword == correctPassword)
        {
            Debug.Log("���� �����.");
        }
        else
        {
            Debug.Log("���� �ƴմϴ�.");
        }

        // �Է� �ʱ�ȭ
        ClearPassword();
    }

    // ��й�ȣ�� �ʱ�ȭ
    private void ClearPassword()
    {
        inputPassword.Clear();
        foreach (GameObject slot in displaySlots)
        {
            // ���Կ� �ƹ��͵� ǥ�õ��� �ʵ��� �ʱ�ȭ
            if (slot.TryGetComponent(out TMPro.TextMeshProUGUI text))
            {
                text.text = "";
            }
        }
    }

    // �Էµ� ��й�ȣ�� ǥ��
    private void UpdateDisplay()
    {
        for (int i = 0; i < displaySlots.Length; i++)
        {
            if (i < inputPassword.Count && displaySlots[i].TryGetComponent(out TMPro.TextMeshProUGUI text))
            {
                text.text = inputPassword[i].ToString();
            }
        }
    }
}
