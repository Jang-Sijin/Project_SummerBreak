using System;
using UnityEngine;

namespace _1.TitleScene.Script
{
    public class UiMenuController : MonoBehaviour
    {
        public AudioClip startAudioClip;
        public AudioClip loadStartAudioClip;
        public AudioClip optionAudioClip;
        public AudioClip quitAudioClip;
        public AudioClip backAudioClip;

        private GameObject _sceneLoader;

        private void Start()
        {
            _sceneLoader = GameObject.Find("SceneManager");
        }


        public void StartButton()
        {
            SoundManager.instance.SfxPlay("StartButton", startAudioClip);
            _sceneLoader.GetComponent<SceneLoader>().enabled = true;
        }

        public void LoadStartButton()
        {
            SoundManager.instance.SfxPlay("LoadStartButton", loadStartAudioClip);
        }

        public void OptionButton()
        {
            SoundManager.instance.SfxPlay("OptionButton", optionAudioClip);
        }

        public void QuitButton()
        {
            SoundManager.instance.SfxPlay("OptionButton", quitAudioClip);
            Application.Quit();
        }
    
        public void BackButton()
        {
            SoundManager.instance.SfxPlay("BackButton", backAudioClip);
        }
    }
}
