using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

//�����ӽ� �ͼ�, �����̴� �ּ�����
public class Sound
{
    public string name; //���� �̸�
}

public class SoundManager : MonoBehaviour
{
    //�̱���. singleton. 1�� (������Ʈ�� �� �ϳ� ����)
    //public Slider bgmSlider;  //bgm�����̴�
    //public Slider sfxSlider;  //sfx�����̴�
    public AudioMixer mixer;
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
        //LoginScenes �̸��̶�� ����Ʈ�� ù ��° ��� �߻�
        if (name0.name == "LoginScenes")
            BGMPlay(bgmList[0]);

        //Stage_n �̸��� Scene��� ȣȯ�ǵ��� �� 4���� ������ ��� �߻�
        //else if (name0.name.StartsWith("Main"))
        //{
            // ���� ����
            //string stageNumberString = new string(name0.name.Where(char.IsDigit).ToArray());
            //if (int.TryParse(stageNumberString, out int stageNumber))
            //{
                // BGM ���� ���� (bgmList[1] ���)
                // BGMPlay(bgmList[1]);

                // �߰�������, Stage ��ȣ�� ���� �ٸ� BGM�� �����Ϸ��� �Ʒ��� ���� Ȯ�� ����:
                // if (stageNumber <= 5)
                //     BGMPlay(bgmList[1]);
                // else if (stageNumber > 5 && stageNumber <= 10)
                //     BGMPlay(bgmList[2]);
            //}
        //}

        //�Ʒ� �ּ� ó���� �κе��� ����� ���������� UI�� ǥ���ϴ� �ڵ��

        // PlayerPrefs���� ������ ����� BGM ���� �� �ҷ�����
        //if (PlayerPrefs.HasKey("BGMVolume"))
        //{
        //    bgmSlider = FindAnyObjectByType<BGMSlider>()?.transform.GetChild(0).GetChild(2).GetComponent<Slider>();

        //    float savedVolume = PlayerPrefs.GetFloat("BGMVolume");
        //    if (bgmSlider)
        //    {
        //        bgmSlider.value = savedVolume; // �����̴� �� ����
        //        BGMSoundVolume(savedVolume); // �ͼ��� �ݿ�
        //    }
        //}

        //bgmSlider?.onValueChanged.RemoveAllListeners();
        //bgmSlider?.onValueChanged.AddListener(BGMSoundVolume);

        // PlayerPrefs���� ������ ����� SFX ���� �� �ҷ�����
        //if (PlayerPrefs.HasKey("SFXVolume"))
        //{
        //    sfxSlider = FindAnyObjectByType<BGMSlider>()?.transform.GetChild(0).GetChild(4).GetComponent<Slider>();

        //    float savedVolume = PlayerPrefs.GetFloat("SFXVolume");
        //    if (sfxSlider)
        //    {
        //        sfxSlider.value = savedVolume; // �����̴� �� ����
        //        SFXSoundVolume(savedVolume); // �ͼ��� �ݿ�
        //    }
        //}

        //sfxSlider?.onValueChanged.RemoveAllListeners();
        //sfxSlider?.onValueChanged.AddListener(SFXSoundVolume);

        //FindAnyObjectByType<BGMSlider>()?.gameObject.SetActive(false);
    }

    //����� �� SoundManager.instance.SFXPlaye(sfxList[]);  �� �߰�
    public void SFXPlay(string sfxName)
    {
        AudioClip clip = System.Array.Find(sfxList, sound => sound.name == sfxName);    //�̸����� ã��

        if (clip != null)
        {
            GameObject go = new GameObject(sfxName + "Sound"); //�������� ������
            AudioSource audiosource = go.AddComponent<AudioSource>();   //AudioSource���۳�Ʈ �߰�
            audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX �ͼ� �߰�
            audiosource.clip = clip;   //clip ����
            audiosource.volume = 1f;    //volume��
            audiosource.Play();     //�װ� ����

            Destroy(go, clip.length);  //ȿ���� �Ҹ� ������ �ı�
        }
    }

    public void SFXSoundVolume(float val)
    {
        // mixer�� log���� ����ϱ� ������ �̷��� ���ϸ� ��������
        float volume = Mathf.Log10(val) * 20;

        // mixer�� ������ ����
        mixer.SetFloat("SFXsound", volume);

        // �����̴��� ���� PlayerPrefs�� ����
        PlayerPrefs.SetFloat("SFXVolume", val);
        PlayerPrefs.Save();
    }

    //BGM Clip�� clip, �ݺ�, �Ҹ�ũ��, play���� �ٷ�� �޼���
    //����� �� SoundManager.instance.BGMPlaye(bgmList[]);  �� �߰�
    public void BGMPlay(AudioClip clip)
    {
        // ���� ��� ���� BGM�� ������ ��� ���� ������� ����
        if (bgmSound.clip == clip && bgmSound.isPlaying)
        {
            return;
        }

        bgmSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];    //Mixer
        bgmSound.clip = clip;
        bgmSound.loop = true;
        bgmSound.volume = 1f;
        bgmSound.Play();
    }

    public void BGMSoundVolume(float val)
    {
        // mixer�� log���� ����ϱ� ������ �̷��� ���ϸ� ��������
        float volume = Mathf.Log10(val) * 20;

        // mixer�� ������ ����
        mixer.SetFloat("BGMsound", volume);

        // �����̴��� ���� PlayerPrefs�� ���� 
        PlayerPrefs.SetFloat("BGMVolume", val);
        PlayerPrefs.Save();
    }

    //FadeOut�ٷ� �� ����� �ڷ�ƾ, �Լ� 
    //����� �� SoundManager.instance.FadeOutBGM(2.0f);  �� �߰� // FadeOut2�� 
    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)    //FadeOutBGM�ڷ�ƾ
    {
        float startVolume;
        mixer.GetFloat("BGMsound", out startVolume);
        startVolume = Mathf.Pow(10, startVolume / 20); // �αװ��� ���������� ��ȯ

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            mixer.SetFloat("BGMsound", Mathf.Log10(newVolume) * 20);
            yield return null;
        }

        // ������ 0�� �� �� BGM ����
        bgmSound.Stop();
        mixer.SetFloat("BGMsound", Mathf.Log10(startVolume) * 20); // �ʱ� ���� ����
    }
}