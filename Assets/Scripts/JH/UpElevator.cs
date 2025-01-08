using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpElevator : MonoBehaviour
{
    [SerializeField] private List<Transform> elevatorDoors; //엘리베이터 4개
    public float openDuration = 2f; //문 열리는 시간
    private Vector3 closedScale = new Vector3(1, 1, 1); //닫힌 상태의 Scale
    private Vector3 openScale = new Vector3(0, 1, 1);   //열린 상태의 Scale

    public bool isUpDoorOpening = false; //문이 열리는 중인지 확인

    [PunRPC]
    public void CmdOpenDoors()
    {
        if (isUpDoorOpening) return;

        isUpDoorOpening = true;
        StartCoroutine(OpenDoorsCoroutine());
    }

    public IEnumerator OpenDoorsCoroutine()
    {
        SoundManager.instance.SFXPlay("Elevator2_SFX");

        yield return new WaitForSeconds(1f);

        SoundManager.instance.SFXPlay("ElevatorDoor2_SFX");

        float elapsedTime = 0f;

        while (elapsedTime < openDuration)
        {
            float t = elapsedTime / openDuration;   //보간 비율 0 ~ 1 사이 값 계산
            for (int i = 0; i < elevatorDoors.Count; i++)
            {
                elevatorDoors[i].localScale = Vector3.Lerp(closedScale, openScale, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;  //다음 프레임까지 대기
        }

        for (int i = 0; i < elevatorDoors.Count; i++)
        {
            elevatorDoors[i].localScale = openScale;
        }
    }
}