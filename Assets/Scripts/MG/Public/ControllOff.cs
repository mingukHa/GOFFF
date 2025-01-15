using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ControllOff : MonoBehaviourPun
{
        private void Start()
        {
            // ���� �÷��̾�� ���� ��Ʈ�ѷ� ����
            if (!photonView.IsMine)
            {
                DisableRemoteController();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return; // ���� ��Ʈ�ѷ� �Է� ����
            }

            // ���� �÷��̾��� �Է� ó��
            if (Input.GetButtonDown("Trigger"))
            {
                Debug.Log("Local controller trigger pressed.");
            }
        }

        private void DisableRemoteController()
        {
            // ������ ��Ȱ��ȭ
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }

            // ��ȣ�ۿ� ��Ȱ��ȭ (XR Interaction Toolkit)
            XRBaseInteractor interactor = GetComponent<XRBaseInteractor>();
            if (interactor != null)
            {
                interactor.enabled = false;
            }

            // �浹 ��Ȱ��ȭ
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            Debug.Log("Remote controller disabled.");
        }
    }

