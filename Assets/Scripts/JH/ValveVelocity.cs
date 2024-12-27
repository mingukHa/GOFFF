using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ValveVelocity : MonoBehaviour
{
    public XRBaseInteractor leftController; // ���� ��Ʈ�ѷ�
    public XRBaseInteractor rightController; // ������ ��Ʈ�ѷ�
    public Transform handle; // ȸ���� �ڵ� ������Ʈ

    private Vector3 previousLeftPosition; // ���� ���� ��Ʈ�ѷ� ��ġ
    private Vector3 previousRightPosition; // ���� ������ ��Ʈ�ѷ� ��ġ

    void Start()
    {
        // ��Ʈ�ѷ� �ʱ� ��ġ ����
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;
    }

    void Update()
    {
        // �� ��Ʈ�ѷ��� ��ġ ���̸� ���
        Vector3 leftDelta = leftController.transform.position - previousLeftPosition;
        Vector3 rightDelta = rightController.transform.position - previousRightPosition;

        // �� ���� �̵� ���̸� �ջ��Ͽ� ȸ�� �� ���
        float rotationAmount = (leftDelta.x + rightDelta.x) * 10f; // ȸ�� �ӵ� ����

        // �ڵ��� ȸ����Ŵ
        handle.Rotate(Vector3.up, rotationAmount);

        // ���� ��ġ�� ������Ʈ
        previousLeftPosition = leftController.transform.position;
        previousRightPosition = rightController.transform.position;
    }
}