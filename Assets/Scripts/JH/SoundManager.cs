using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

//본게임시 믹서, 슬라이더 주석해제
public class Sound
{
    public string name; //곡의 이름
}

public class SoundManager : MonoBehaviour
{
    //싱글턴. singleton. 1개 (프로젝트에 단 하나 존재)
    //public Slider bgmSlider;  //bgm슬라이더
    //public Slider sfxSlider;  //sfx슬라이더
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
        //LoginScenes 이름이라면 리스트의 첫 번째 브금 발생
        if (name0.name == "LoginScenes")
            BGMPlay(bgmList[0]);

        //Stage_n 이름의 Scene들과 호환되도록 앞 4글자 동일한 브금 발생
        //else if (name0.name.StartsWith("Main"))
        //{
            // 숫자 추출
            //string stageNumberString = new string(name0.name.Where(char.IsDigit).ToArray());
            //if (int.TryParse(stageNumberString, out int stageNumber))
            //{
                // BGM 선택 로직 (bgmList[1] 재생)
                // BGMPlay(bgmList[1]);

                // 추가적으로, Stage 번호에 따라 다른 BGM을 설정하려면 아래와 같이 확장 가능:
                // if (stageNumber <= 5)
                //     BGMPlay(bgmList[1]);
                // else if (stageNumber > 5 && stageNumber <= 10)
                //     BGMPlay(bgmList[2]);
            //}
        //}

        //아래 주석 처리된 부분들은 저장된 볼륨값들을 UI에 표시하는 코드들

        // PlayerPrefs에서 이전에 저장된 BGM 볼륨 값 불러오기
        //if (PlayerPrefs.HasKey("BGMVolume"))
        //{
        //    bgmSlider = FindAnyObjectByType<BGMSlider>()?.transform.GetChild(0).GetChild(2).GetComponent<Slider>();

        //    float savedVolume = PlayerPrefs.GetFloat("BGMVolume");
        //    if (bgmSlider)
        //    {
        //        bgmSlider.value = savedVolume; // 슬라이더 값 복원
        //        BGMSoundVolume(savedVolume); // 믹서에 반영
        //    }
        //}

        //bgmSlider?.onValueChanged.RemoveAllListeners();
        //bgmSlider?.onValueChanged.AddListener(BGMSoundVolume);

        // PlayerPrefs에서 이전에 저장된 SFX 볼륨 값 불러오기
        //if (PlayerPrefs.HasKey("SFXVolume"))
        //{
        //    sfxSlider = FindAnyObjectByType<BGMSlider>()?.transform.GetChild(0).GetChild(4).GetComponent<Slider>();

        //    float savedVolume = PlayerPrefs.GetFloat("SFXVolume");
        //    if (sfxSlider)
        //    {
        //        sfxSlider.value = savedVolume; // 슬라이더 값 복원
        //        SFXSoundVolume(savedVolume); // 믹서에 반영
        //    }
        //}

        //sfxSlider?.onValueChanged.RemoveAllListeners();
        //sfxSlider?.onValueChanged.AddListener(SFXSoundVolume);

        //FindAnyObjectByType<BGMSlider>()?.gameObject.SetActive(false);
    }

    //사용할 때 SoundManager.instance.SFXPlaye(sfxList[]);  를 추가
    public void SFXPlay(string sfxName)
    {
        AudioClip clip = System.Array.Find(sfxList, sound => sound.name == sfxName);    //이름으로 찾기

        if (clip != null)
        {
            GameObject go = new GameObject(sfxName + "Sound"); //사운드파일 가져옴
            AudioSource audiosource = go.AddComponent<AudioSource>();   //AudioSource컴퍼넌트 추가
            audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX 믹서 추가
            audiosource.clip = clip;   //clip 파일
            audiosource.volume = 1f;    //volume값
            audiosource.Play();     //그걸 실행

            Destroy(go, clip.length);  //효과음 소리 지나면 파괴
        }
    }

    public void SFXSoundVolume(float val)
    {
        // mixer는 log값을 사용하기 때문에 이렇게 안하면 문제생김
        float volume = Mathf.Log10(val) * 20;

        // mixer에 볼륨을 적용
        mixer.SetFloat("SFXsound", volume);

        // 슬라이더의 값을 PlayerPrefs에 저장
        PlayerPrefs.SetFloat("SFXVolume", val);
        PlayerPrefs.Save();
    }

    //BGM Clip의 clip, 반복, 소리크기, play값을 다루는 메서드
    //사용할 때 SoundManager.instance.BGMPlaye(bgmList[]);  를 추가
    public void BGMPlay(AudioClip clip)
    {
        // 현재 재생 중인 BGM과 동일한 경우 새로 재생하지 않음
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
        // mixer는 log값을 사용하기 때문에 이렇게 안하면 문제생김
        float volume = Mathf.Log10(val) * 20;

        // mixer에 볼륨을 적용
        mixer.SetFloat("BGMsound", volume);

        // 슬라이더의 값을 PlayerPrefs에 저장 
        PlayerPrefs.SetFloat("BGMVolume", val);
        PlayerPrefs.Save();
    }

    //FadeOut다룰 때 사용할 코루틴, 함수 
    //사용할 때 SoundManager.instance.FadeOutBGM(2.0f);  를 추가 // FadeOut2초 
    public void FadeOutBGM(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeOutCoroutine(float duration)    //FadeOutBGM코루틴
    {
        float startVolume;
        mixer.GetFloat("BGMsound", out startVolume);
        startVolume = Mathf.Pow(10, startVolume / 20); // 로그값을 선형값으로 변환

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            mixer.SetFloat("BGMsound", Mathf.Log10(newVolume) * 20);
            yield return null;
        }

        // 볼륨이 0이 된 후 BGM 정지
        bgmSound.Stop();
        mixer.SetFloat("BGMsound", Mathf.Log10(startVolume) * 20); // 초기 볼륨 복원
    }
}