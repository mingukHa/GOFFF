using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

//본게임시 믹서, 슬라이더 주석해제
//public class Sound
//{
//    public string name; //곡의 이름
//}

public class SoundManager : MonoBehaviourPunCallbacks
{
    //싱글턴. singleton. 1개 (프로젝트에 단 하나 존재)
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
        // 기존 재생 중인 배경음악 멈추기
        if (bgmSound.isPlaying)
        {
            bgmSound.Stop();
        }
        //LoginScenes 이름이라면 리스트의 첫 번째 브금 발생
        if (name0.name == "LoginScenes")
            BGMPlay(bgmList[0]);
        else if (name0.name == "waitRoom")
            BGMPlay(bgmList[0]);
        else if (name0.name == "MainScenes")
            BGMPlay(bgmList[1]);
    }

    //사용할 때 SoundManager.instance.SFXPlaye(sfxList[]);  를 추가
    public void SFXPlay(string sfxName)
    {
        AudioClip clip = System.Array.Find(sfxList, sound => sound.name == sfxName);    //이름으로 찾기

        if (clip != null)
        {
            GameObject go = new GameObject(sfxName + "Sound"); //사운드파일 가져옴
            //audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0]; //SFX 믹서 추가
            AudioSource audiosource = go.AddComponent<AudioSource>();   //AudioSource컴퍼넌트 추가

            // 이미 동일한 사운드가 재생 중이라면 멈추기
            if (sfxSound.isPlaying)
            {
                sfxSound.Stop(); // 현재 재생 중인 사운드를 중지
            }

            // 3D 효과 설정
            audiosource.spatialBlend = 1f;         // 1: 3D 사운드, 0: 2D 사운드
            audiosource.minDistance = 1f;          // 최소 거리 (풀 볼륨)
            audiosource.maxDistance = 20f;         // 최대 거리 (감쇠 시작)
            audiosource.rolloffMode = AudioRolloffMode.Linear;  // 선형 감쇠

            audiosource.clip = clip;   //clip 파일
            audiosource.volume = 1f;    //volume값

            audiosource.Play();     //그걸 실행

            Destroy(go, clip.length);  //효과음 소리 지나면 파괴
        }
    }

    public void BGMPlay(AudioClip clip)
    {
        // 현재 재생 중인 BGM과 동일한 경우 새로 재생하지 않음
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

    // 좀비에게 발각된 상태에서 호출될 메서드
    [PunRPC]
    public void OnZombieDetected()
    {
        // 기존 배경 음악의 볼륨을 0으로 설정 (음악이 멈추지 않고 볼륨만 0)
        if (bgmSound.isPlaying)
        {
            bgmSound.volume = 0f;
        }

        // 좀비에게 발각되면 bgmList[2]을 재생하고, 기존 음악의 볼륨을 0으로 설정
        BGMPlay(bgmList[2]);
    }

    // 좀비에게서 벗어난 상태에서 호출될 메서드
    [PunRPC]
    public void OnZombieLost()
    {
        // 기존 배경 음악의 볼륨을 0으로 설정 (음악이 멈추지 않고 볼륨만 0)
        if (bgmSound.isPlaying)
        {
            bgmSound.volume = 0f;
        }

        // 좀비에게서 벗어나면 bgmList[1]을 재생하고, 볼륨을 다시 1로 설정
        BGMPlay(bgmList[1]);

        // bgmSound.volume을 1로 복원
        bgmSound.volume = 1f;
    }
}