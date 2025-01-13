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
            foreach (var renderer in GetComponentsInChildren<Renderer>()) //�迭�� ���Ƽ� �������� ã�´�
            {
                renderer.enabled = false; //�������� ����
            }

            // ��ȣ�ۿ� ��Ȱ��ȭ (XR Interaction Toolkit)
            XRBaseInteractor interactor = GetComponent<XRBaseInteractor>(); //�Է�
            if (interactor != null)
            {
                interactor.enabled = false;//���� �ʱ�
            }

            // �浹 ��Ȱ��ȭ
            foreach (var collider in GetComponentsInChildren<Collider>()) //�浹
            {
                collider.enabled = false;//���� �ʱ�
            }

        }
    }

