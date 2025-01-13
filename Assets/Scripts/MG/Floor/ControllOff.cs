using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ControllOff : MonoBehaviourPun
{
        private void Start()
        {
            // 로컬 플레이어와 상대방 컨트롤러 구분
            if (!photonView.IsMine)
            {
                DisableRemoteController();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return; // 상대방 컨트롤러 입력 무시
            }

            // 로컬 플레이어의 입력 처리
            if (Input.GetButtonDown("Trigger"))
            {
                Debug.Log("Local controller trigger pressed.");
            }
        }

        private void DisableRemoteController()
        {
            // 렌더링 비활성화
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }

            // 상호작용 비활성화 (XR Interaction Toolkit)
            XRBaseInteractor interactor = GetComponent<XRBaseInteractor>();
            if (interactor != null)
            {
                interactor.enabled = false;
            }

            // 충돌 비활성화
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            Debug.Log("Remote controller disabled.");
        }
    }

