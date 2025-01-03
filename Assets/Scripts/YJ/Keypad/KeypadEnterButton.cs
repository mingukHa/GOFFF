using UnityEngine;

public class KeypadEnterButton : MonoBehaviour
{
    private KeypadManager keypadManager;

    private void Start()
    {
        keypadManager = FindFirstObjectByType<KeypadManager>();
    }

    public void OnPressed()
    {
        keypadManager.OnEnterButtonPressed();
    }
}
