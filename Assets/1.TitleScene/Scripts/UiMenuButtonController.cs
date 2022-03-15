using System;
using UnityEngine;

namespace _1.TitleScene.Script
{
    public class UiMenuButtonController : MonoBehaviour
    {
        public AudioClip startAudioClip;
        public AudioClip loadStartAudioClip;
        public AudioClip clickAudioClip;
        public AudioClip quitAudioClip;
        public AudioClip backAudioClip;

        private GameObject _sceneLoader;

        private void Start()
        {
            _sceneLoader = GameObject.Find("SceneManager");
        }

        public void LoadStartButton()
        {
            SoundManager.instance.SfxPlay("LoadStartButton", loadStartAudioClip);
            _sceneLoader.GetComponent<SceneLoader>().enabled = true;
            XMLManager.instance.LoadByXML();
        }

        public void StartButton()
        {
            SoundManager.instance.SfxPlay("StartButton", startAudioClip);
            _sceneLoader.GetComponent<SceneLoader>().enabled = true;
        }

        public void ClickButton()
        {
            SoundManager.instance.SfxPlay("ClickButton", clickAudioClip);
        }

        public void QuitButton()
        {
            SoundManager.instance.SfxPlay("ClickButton", quitAudioClip);
            Application.Quit();
        }
    
        public void BackButton()
        {
            SoundManager.instance.SfxPlay("BackButton", backAudioClip);
        }
    }
}