using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1.TitleScene.Script
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource backGroundSound;
    
        [Header("BGM(배경음)의 파일명은 BGM_으로 설정해주세요.")]
        public AudioClip[] backGroundList; // 배경음 리스트
    
        public static SoundManager instance; // Sound Manager을 싱글톤으로 관리
        private void Awake()
        {
            // Sound Manager 싱글톤 설정
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
                SceneManager.sceneLoaded += OnSceneLoaded;
            } 
            else
            { 
                Destroy(gameObject);  
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) // Scene에 따라서 Bg Sound가 변경된다.
        {
            foreach (var bgmListIndex in backGroundList)
            {
                string bgmName = bgmListIndex.name.Substring(0,4);
                Debug.Log($"[장시진] {arg0.name} Scene의 BGM 파일명: {bgmListIndex.name}");
            
                if ("BGM_" == bgmName) // 배경음 이름의 앞 4글자가 BGM_으로 시작하면
                {
                    BgSoundPlay(bgmListIndex); // 배경음 실행
                }
                else // 배경음 이름의 앞 4글자가 BGM_으로 시작하지 않으면 배경음 없도록(null) 설정
                {
                    Debug.Log($"[장시진] {arg0.name} Scene의 BGM 파일이 없거나 BGM 파일명 형식에 오류가 있습니다.");
                    BgSoundPlay(null);
                }
            }
        }

        public void SfxPlay(string sfxName, AudioClip clip)
        {
            GameObject go = new GameObject(sfxName + "Sound"); // Sound GameObject 생성
            AudioSource audioSource = go.AddComponent<AudioSource>(); // 사운드 재생을 위해서 AudioSource 컴포넌트를 추가
            audioSource.clip = clip; // 오디오 클립을 매개변수로 받아온다.
            audioSource.Play(); // 오디오 재생
        
            Destroy(go, clip.length); // Destroy(파괴할 오브젝트, 지연시간); // 오디오를 끝까지 재생하면 오브젝트를 파괴한다.
        }

        public void BgSoundPlay(AudioClip clip)
        {
            backGroundSound.clip = clip; // 배경음 오디오 클립 설정
            backGroundSound.loop = true; // 반복 모드
            backGroundSound.volume = 1.0f; // 배경음 소리 값 설정
            backGroundSound.Play(); // 배경음 실행
        }
    }
}
