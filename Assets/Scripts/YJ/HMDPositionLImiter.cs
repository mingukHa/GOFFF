using UnityEngine;
using UnityEngine.XR;

public class HMDPositionLimiter : MonoBehaviour
{
    public float maxDistance = 0.1f; // �̵� ��� ���� (����)
    private Vector3 initialPosition; // �ʱ� ��ġ
    public Transform hmdTransform; // HMD�� Transform

    void Start()
    {
        // �ʱ� ��ġ ���� (�÷��̾ ������ ������ ��ġ)
        hmdTransform = Camera.main.transform; // HMD ī�޶��� Transform
        initialPosition = hmdTransform.localPosition; // ���� ��ġ�� �������� ���
    }

    void Update()
    {
        // ���� HMD ��ġ ��������
        Vector3 currentPosition = hmdTransform.localPosition;

        // �ʱ� ��ġ���� �Ÿ� ���
        Vector3 offset = currentPosition - initialPosition;

        // �Ÿ��� ���� ������ �ʰ��ϸ� ����
        if (offset.magnitude > maxDistance)
        {
            // ������ ������ ä�� ���� ���� ���� �̵�
            offset = offset.normalized * maxDistance;
            hmdTransform.localPosition = initialPosition + offset;
        }
    }
}
