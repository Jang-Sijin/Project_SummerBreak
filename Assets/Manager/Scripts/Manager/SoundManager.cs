using System;
using MiscUtil.Extensions.TimeRelated;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("[BGM AudioClip Settings]")] 
    [SerializeField] private SoundDB _bgmDB;

    [Header("[UI AudioClip Settings]")]
    [SerializeField] private SoundDB _sfxDB;

    [Header("[AudioMixer]")]
    public AudioMixer Mixer;
    
    [Header("[AudioSource]")]
    public AudioSource BgmSource;
    public AudioSource SfxSource;
 
    [Header("BGM(배경음)의 파일명은 Scene의 맨앞 숫자로 설정해주세요.")]
    [Header("[예시: 1.TitleScene -> 1_BGM_Something]")]
    public AudioClip[] backGroundList; // 배경음 리스트
    [Header("[배경음, 효과음 초기 사운드 크기 값]")]
    [Header("값 변경 시 Title Scene의 UI-AudioSetting_Menu-Slider Bar의 값도 수정해야 합니다.")]
    public static float BGstartVolumeValue = 0.5f; // 배경음 사운드 크기 초기값은 0.5로 설정
    public static float SFXstartVolumeValue = 0.5f; // 효과음 사운드 크기 초기값은 0.5로 설정
    private void Start()
    {
        // 배경음, 효과음 초기값 설정
        Mixer.SetFloat("BackGroundSound", Mathf.Log10(BGstartVolumeValue) * 20);
        Mixer.SetFloat("SFXSound", Mathf.Log10(SFXstartVolumeValue) * 20);

        PlayBGM(0);
        // SoundManagerOld.Instance.PlaySFX();
        // SoundManagerOld.Instance.PlayBGM();
        // SoundManagerOld.Instance.PlaySFX();
    }
    #region Singleton
    public static SoundManager Instance; // Sound Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Sound Manager 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            
            // SceneManager.sceneLoaded += OnSceneLoaded;
        } 
        else
        { 
            Destroy(gameObject);
        }
    }
    #endregion Singleton
    
    // --------------------------------------------------
    // [BGM Sound Functions]
    
    public void PlayBGM(int soundIndex)
    {
        BgmSource.clip = _bgmDB.soundAudioClip[soundIndex];
        BgmSource.Play();
    }
    
    public void PlayBGM(string soundName)
    {
        int findIndex = Array.IndexOf(_bgmDB.soundName, soundName);
        BgmSource.clip = _bgmDB.soundAudioClip[findIndex];
        BgmSource.Play();
    }


    // --------------------------------------------------
    // [BGM Sound Functions]
    
    public void PlaySFX(int soundIndex)
    {
        SfxSource.PlayOneShot(_sfxDB.soundAudioClip[soundIndex]);
    }

    public void PlaySFX(string soundName)
    {
        int findIndex = Array.IndexOf(_sfxDB.soundName, soundName);
        SfxSource.PlayOneShot(_sfxDB.soundAudioClip[findIndex]);
    }
    
    // --------------------------------------------------
    
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) // Scene에 따라서 Bg Sound가 변경된다.
    {
        if (BgmSource.clip != null)
        {
            // 씬이 이동될 때 현재 실행중인 배경음을 제외한다.
            BgmSource.clip = null;
        }
        
        foreach (var bgmListIndex in backGroundList)
        {
            string bgmName = bgmListIndex.name.ToString();
            print($"{bgmName}");
            if (arg0.name.ToString() == bgmName) // 배경음 BGM 이름이 씬 이름과 같을 때
            {
                Debug.Log($"[장시진] {arg0.name} Scene의 BGM 파일명: {bgmListIndex.name}");
                BgSoundPlay(bgmListIndex); // 배경음 실행
                break;
            }
            else if (bgmListIndex == backGroundList[backGroundList.Length - 1])
            {
                Debug.Log($"[장시진] {arg0.name} Scene의 BGM 파일이 없거나 BGM 파일명 형식에 오류가 있습니다.");   
            }
        }
    }
    
    public void SfxPlay(string sfxName, AudioClip clip) // 효과음
    {
        GameObject soundObj = new GameObject(sfxName + "Sound"); // Sound GameObject 생성
        AudioSource audioSource = soundObj.AddComponent<AudioSource>(); // 사운드 재생을 위해서 AudioSource 컴포넌트를 추가
        audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFXSound")[0];
        audioSource.clip = clip; // 오디오 클립을 매개변수로 받아온다.
        // audioSource.volume = SFXstartVolumeValue; // 효과음 소리 크기값 설정 [X]
        audioSource.Play(); // 오디오 재생
    
        Destroy(soundObj, clip.length); // Destroy(파괴할 오브젝트, 지연시간); // 오디오를 끝까지 재생하면 오브젝트를 파괴한다.
    }
    public void BgSoundPlay(AudioClip clip) // 배경음
    {
        BgmSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("BackGroundSound")[0];
        BgmSource.clip = clip; // 배경음 오디오 클립 설정
        BgmSource.loop = true; // 반복 모드
        // BgmSource.volume = BGstartVolumeValue; // 배경음 소리 크기값 설정
        BgmSource.Play(); // 배경음 실행
    }
    #region 사운드바 컨트롤 설정
    // 배경음 컨트롤
    public void BgSoundVolume(float volumeValue)
    {
        // mixer의 볼륨은 log scale값으로 설정되어 있다.
        Mixer.SetFloat("BackGroundSound", Mathf.Log10(volumeValue) * 20);
    }
    // 효과음 컨트롤
    public void SfxSoundVolume(float volumeValue)
    {
        // mixer의 볼륨은 log scale값으로 설정되어 있다.
        Mixer.SetFloat("SFXSound", Mathf.Log10(volumeValue) * 20);
    }
    public float GetBgSoundVolumeValue()
    {
        float volumeValue;
        Mixer.GetFloat("BackGroundSound", out volumeValue);
        return Mathf.Pow(10,volumeValue/ 20.0f);
    }
    
    public float GetSfxSoundVolumeValue()
    {
        float volumeValue;
        Mixer.GetFloat("SFXSound", out volumeValue);
        return Mathf.Pow(10,volumeValue/ 20.0f);
    }
    #endregion 사운드바 컨트롤 설정
 }