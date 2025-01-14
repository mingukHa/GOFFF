using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

//�����ӽ� �ͼ�, �����̴� �ּ�����
//public class Sound
//{
//    public string name; //���� �̸�
//}

public class SoundManager : MonoBehaviourPunCallbacks
{
    //�̱���. singleton. 1�� (������Ʈ�� �� �ϳ� ����)
    public AudioSource bgmSound;
    public AudioSource sfxSound;
    public static SoundManager instance;

    public AudioClip[] bgmList;
    public AudioClip[] sfxList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene name0, LoadSceneMode name1)
    {
        // ���� ��� ���� ������� ���߱�
        if (bgmSound.isPlaying)
        {
            bgmSound.Stop();
        }
        //LoginScenes �̸��̶�� ����Ʈ�� ù ��° ��� �߻�
        if (name0.name == "LoginScenes")
            BGMPlay(bgmList[0]);
        else if (name0.name == "waitRoom")
            BGMPlay(bgmList[0]);
        else if (name0.name == "MainScenes")
            BGMPlay(bgmList[1]);
    }

    //����� �� SoundManager.instance.SFXPlaye(sfxList[]);  �� �߰�
    public void SFXPlay(string sfxName)
    {
        AudioClip clip = System.Array.Find(sfxList, sound => sound.name == sfxName);    //�̸����� ã��

        if (clip != null)
        {
            GameObject go = new GameObject(sfxName + "Sound"); //�������� ������
            //audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX �ͼ� �߰�
            AudioSource audiosource = go.AddComponent<AudioSource>();   //AudioSource���۳�Ʈ �߰�

            // �̹� ������ ���尡 ��� ���̶�� ���߱�
            if (sfxSound.isPlaying)
            {
                sfxSound.Stop(); // ���� ��� ���� ���带 ����
            }

            // 3D ȿ�� ����
            audiosource.spatialBlend = 1f;         // 1: 3D ����, 0: 2D ����
            audiosource.minDistance = 1f;          // �ּ� �Ÿ� (Ǯ ����)
            audiosource.maxDistance = 20f;         // �ִ� �Ÿ� (���� ����)
            audiosource.rolloffMode = AudioRolloffMode.Linear;  // ���� ����

            audiosource.clip = clip;   //clip ����
            audiosource.volume = 1f;    //volume��

            audiosource.Play();     //�װ� ����

            Destroy(go, clip.length);  //ȿ���� �Ҹ� ������ �ı�
        }
    }

    public void BGMPlay(AudioClip clip)
    {
        // ���� ��� ���� BGM�� ������ ��� ���� ������� ����
        if (bgmSound.clip == clip && bgmSound.isPlaying)
        {
            return;
        }

        //bgmSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];    //Mixer
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 1f;
        bgmSound.Play();
    }

    // ���񿡰� �߰��� ���¿��� ȣ��� �޼���
    [PunRPC]
    public void OnZombieDetected()
    {
        // ���� ��� ������ ������ 0���� ���� (������ ������ �ʰ� ������ 0)
        if (bgmSound.isPlaying)
        {
            bgmSound.volume = 0f;
        }

        // ���񿡰� �߰��Ǹ� bgmList[2]�� ����ϰ�, ���� ������ ������ 0���� ����
        BGMPlay(bgmList[2]);
    }

    // ���񿡰Լ� ��� ���¿��� ȣ��� �޼���
    [PunRPC]
    public void OnZombieLost()
    {
        // ���� ��� ������ ������ 0���� ���� (������ ������ �ʰ� ������ 0)
        if (bgmSound.isPlaying)
        {
            bgmSound.volume = 0f;
        }

        // ���񿡰Լ� ����� bgmList[1]�� ����ϰ�, ������ �ٽ� 1�� ����
        BGMPlay(bgmList[1]);

        // bgmSound.volume�� 1�� ����
        bgmSound.volume = 1f;
    }
}