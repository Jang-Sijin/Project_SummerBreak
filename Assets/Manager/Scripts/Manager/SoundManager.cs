using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


    public class SoundManager : MonoBehaviour
    {
        public AudioMixer mixer;
        public AudioSource backGroundSound;
    
        [Header("BGM(배경음)의 파일명은 Scene의 맨앞 숫자로 설정해주세요.")]
        [Header("[예시: 1.TitleScene -> 1_BGM_Something]")]
        public AudioClip[] backGroundList; // 배경음 리스트

        [Header("[배경음, 효과음 초기 사운드 크기 값]")]
        [Header("값 변경 시 Title Scene의 UI-AudioSetting_Menu-Slider Bar의 값도 수정해야 합니다.")]
        public float BGstartVolumeValue = 0.5f; // 배경음 사운드 크기 초기값은 0.5로 설정
        public float SFXstartVolumeValue = 0.5f; // 효과음 사운드 크기 초기값은 0.5로 설정

        private void Start()
        {
            // 배경음, 효과음 초기값 설정
            mixer.SetFloat("BackGroundSound", Mathf.Log10(BGstartVolumeValue) * 20);
            mixer.SetFloat("SFXSound", Mathf.Log10(SFXstartVolumeValue) * 20);
        }

        #region Singleton
        public static SoundManager instance; // Sound Manager을 싱글톤으로 관리
        private void Awake()
        {
            // Sound Manager 싱글톤 설정
            if (instance == null)
            {
                instance = this;
                // DontDestroyOnLoad(instance);
                SceneManager.sceneLoaded += OnSceneLoaded;
            } 
            else
            { 
                Destroy(gameObject);
            }
        }
        #endregion Singleton

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) // Scene에 따라서 Bg Sound가 변경된다.
        {
            foreach (var bgmListIndex in backGroundList)
            {
                string bgmName = bgmListIndex.name.Substring(0,1);

                if (arg0.name[1].ToString() == bgmName) // 배경음 이름의 앞 4글자가 BGM_으로 시작하면
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
            audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFXSound")[0];
            audioSource.clip = clip; // 오디오 클립을 매개변수로 받아온다.
            // audioSource.volume = SFXstartVolumeValue; // 효과음 소리 크기값 설정 [X]
            audioSource.Play(); // 오디오 재생
        
            Destroy(soundObj, clip.length); // Destroy(파괴할 오브젝트, 지연시간); // 오디오를 끝까지 재생하면 오브젝트를 파괴한다.
        }

        public void BgSoundPlay(AudioClip clip) // 배경음
        {
            backGroundSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BackGroundSound")[0];
            backGroundSound.clip = clip; // 배경음 오디오 클립 설정
            backGroundSound.loop = true; // 반복 모드
            // backGroundSound.volume = BGstartVolumeValue; // 배경음 소리 크기값 설정
            backGroundSound.Play(); // 배경음 실행
        }

        #region 사운드바 컨트롤 설정

        // 배경음 컨트롤
        public void BgSoundVolume(float volumeValue)
        {
            // mixer의 볼륨은 log scale값으로 설정되어 있다.
            mixer.SetFloat("BackGroundSound", Mathf.Log10(volumeValue) * 20);
        }
        
        // 효과음 컨트롤
        public void SfxSoundVolume(float volumeValue)
        {
            // mixer의 볼륨은 log scale값으로 설정되어 있다.
            mixer.SetFloat("SFXSound", Mathf.Log10(volumeValue) * 20);
        }

        #endregion 사운드바 컨트롤 설정
    }