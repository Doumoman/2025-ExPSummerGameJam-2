using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // 만약 플레이어 프렙스에 저장된 bgm과 effect의 Volume값이 있다면 불러온다. 게임이 꺼졌다 켜져도 전의 값을 유지하기 위함.
        if (!PlayerPrefs.HasKey("bgmVolume")) PlayerPrefs.SetFloat("bgmVolume", 1.0f);
        if (!PlayerPrefs.HasKey("effectVolume")) PlayerPrefs.SetFloat("effectVolume", 1.0f);

        Init();
    }

    AudioClip bgmMain; // 메인 오디오클립
    AudioClip bgmStage1; // 스테이지 오디오클립
    AudioClip bgmStage2;
    AudioClip bgmStage3;
    AudioClip win;
    AudioClip lose;
    AudioClip ending;
    private AudioSource audioSource1; // 배경음 오디오소스, 배경음들을 저장해서 사용함
    private AudioSource audioSource2; // 효과음 오디오소스, 효과음들을 저장해서 사용함


    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public AudioMixer audioMixer;

    public float currentBGMVolume { get; set; }
    public float currentEffectVolume { get; set; }

    public void Init()
    {
        currentBGMVolume = PlayerPrefs.GetFloat("bgmVolume");
        currentEffectVolume = PlayerPrefs.GetFloat("effectVolume");
        audioMixer = Resources.Load<AudioMixer>("NewMixer");
        AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

        //GameObject root = GameObject.Find("@Sound");
        //root = new GameObject { name = "@Sound" };
        GameObject root = this.gameObject;
        //root.AddComponent<SoundManager>();
        //Object.DontDestroyOnLoad(root);

        string[] soundNames = System.Enum.GetNames(typeof(Sound));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
            go.GetComponent<AudioSource>().outputAudioMixerGroup = audioMixerGroups[i + 1];
        }

        _audioSources[(int)Sound.Bgm].loop = true;
        _audioSources[(int)Sound.LoopEffect].loop = true;

    }
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
        {
            return;
        }
        if (type == Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.volume = PlayerPrefs.GetFloat("bgmVolume");
            audioSource.Play();
        }
        else if (type == Sound.LoopEffect)
        {
            AudioSource audioSource = _audioSources[(int)Sound.LoopEffect];
            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.volume = PlayerPrefs.GetFloat("effectVolume");
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];

            audioSource.pitch = pitch;
            audioSource.volume = PlayerPrefs.GetFloat("effectVolume");
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }
    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";
        AudioClip audioClip = null;

        if (type == Sound.Bgm)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = GameManager.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing {path}");

        return audioClip;
    }
    public bool isBGMPlaying()
    {
        return _audioSources[(int)Sound.Bgm].isPlaying;
    }
    public void StopLoopEffect()
    {
        AudioSource audioSource = _audioSources[(int)Sound.LoopEffect];
        audioSource.clip = null;
        audioSource.Stop();
    }
    
    void Start() // 게임 처음 시작시 음악세팅
    {

        // audioSource에 AudioSource 컴포넌트를 추가
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource1.loop = true;

        // 오디오 클립에 오디오 추가
        bgmMain = Resources.Load<AudioClip>("Sounds/main");
        bgmStage1 = Resources.Load<AudioClip>("Sounds/1stageTheme_first_dream");
        bgmStage2 = Resources.Load<AudioClip>("Sounds/2stageTheme_foggy_classroom");
        bgmStage3 = Resources.Load<AudioClip>("Sounds/3stage");
        win = Resources.Load<AudioClip>("Sounds/win");
        lose = Resources.Load<AudioClip>("Sounds/lose");
        ending = Resources.Load<AudioClip>("Sounds/EndingBGM");


        MainBgmOn(); // 게임 시작시 메인메뉴에서 오프닝Bgm 재생
    }

    public void MainBgmOn()
    {
        audioSource1.clip = bgmMain;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume"); // 플레이어프렙스에서 bgmVolume 값 가져오기
        audioSource1.Play();
    }
    public void Stage1BgmOn()
    {
        audioSource1.clip = bgmStage1;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    public void Stage2BgmOn()
    {
        audioSource1.clip = bgmStage2;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    public void Stage3BgmOn()
    {
        audioSource1.clip = bgmStage3;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    public void winBgmOn()
    {
        audioSource1.clip = win;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    public void loseBgmOn()
    {
        audioSource1.clip = lose;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    public void EndingBgmOn()
    {
        audioSource1.clip = ending;
        audioSource1.volume = PlayerPrefs.GetFloat("bgmVolume");
        audioSource1.Play();
    }
    //옵션창 음향 슬라이더에서 값 변경시 오디오소스의 볼륨을 조절하고 이 값을 플레이어 프렙스에 저장
    public void OnBgmVolumeChange(float volume)
    {
        audioSource1.volume = volume;
        PlayerPrefs.SetFloat("bgmVolume", volume);
    }
    public void OnEffectVolumeChange(float volume)
    {
        audioSource2.volume = volume;
        PlayerPrefs.SetFloat("effectVolume", volume);
    }

    // 원하는 곳에 효과음 추가 위한 함수
    // SoundManager.Instance.EffectSoundOn("Walk")와 같이 사용
    public void EffectSoundOn(string effectName)
    {
        string effect = "Sounds/" + effectName;
        AudioClip effectClip = Resources.Load<AudioClip>(effect);
        audioSource2.volume = PlayerPrefs.GetFloat("effectVolume"); // 플레이어프렙스에서 effectVolume 값 가져오기
        audioSource2.clip = effectClip;
        audioSource2.PlayOneShot(effectClip);
    }

    public void EffectSoundOff()
    {
        audioSource2.Stop();
    }
}
