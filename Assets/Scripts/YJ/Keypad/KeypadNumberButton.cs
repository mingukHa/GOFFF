using Photon.Pun;
using UnityEngine;

public class KeypadNumberButton : MonoBehaviour
{
    public int number; // 이 버튼이 나타내는 숫자
    //public Transform screenDisplayPoint; // 숫자가 표시될 화면의 부모 Transform
    //public GameObject numberPrefab; // 표시할 숫자 프리팹
    private KeypadManager keypadManager; // 키패드 매니저

    private void Start()
    {
        keypadManager = FindFirstObjectByType<KeypadManager>();
    }

    public void OnPressed()
    {
        // 키패드 누르는 SFX 추가
        Debug.Log(number + "번 버튼을 눌렀습니다.");

        // RPC를 통해 숫자 추가 동작을 동기화
        PhotonView photonView = PhotonView.Get(keypadManager);
        photonView.RPC("AddNumber", RpcTarget.Others, number);
    }
}
