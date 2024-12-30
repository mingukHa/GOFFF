using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private void Start()
    {

        VivoxController.Instance.OnLoginEndEvent += HandleLoginEnd;

    }

    //�ʱ�ȭ�� ���� �� �� ��ȯ
    private void HandleLoginEnd()
    {

        SceneManager.LoadScene("Game");

    }
}