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

        public void LoadStartButton()
        {
            SoundManager.Instance.SfxPlay("LoadStartButton", loadStartAudioClip);
            JsonManager.Instance.Load();
            SceneLoaderManager.instance.LoadGameScene();
            // XMLManager.Instance.LoadByXML();
        }

        public void StartButton()
        {
            SoundManager.Instance.SfxPlay("StartButton", startAudioClip);
            SceneLoaderManager.instance.LoadGameScene();
        }

        public void ClickButton()
        {
            SoundManager.Instance.SfxPlay("ClickButton", clickAudioClip);
        }

        public void QuitButton()
        {
            SoundManager.Instance.SfxPlay("ClickButton", quitAudioClip);
            Application.Quit();
        }
    
        public void BackButton()
        {
            SoundManager.Instance.SfxPlay("BackButton", backAudioClip);
        }
    }
}
