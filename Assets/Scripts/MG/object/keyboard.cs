using UnityEngine;

public class keyboard : MonoBehaviour
{
    [SerializeField]
    private GameObject input;
    [SerializeField]
    private GameObject keyboards; 
    public void OnSelect()
    {
        input.SetActive(true);
    }
    public void OnClose()
    {
        keyboards.SetActive(false);
    }
}
