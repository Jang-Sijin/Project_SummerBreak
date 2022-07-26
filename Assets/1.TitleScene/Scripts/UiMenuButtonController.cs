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
            SoundManagerOld.instance.SfxPlay("LoadStartButton", loadStartAudioClip);
            JsonManager.instance.Load();
            SceneLoaderManager.instance.LoadGameScene();
            // XMLManager.instance.LoadByXML();
        }

        public void StartButton()
        {
            SoundManagerOld.instance.SfxPlay("StartButton", startAudioClip);
            SceneLoaderManager.instance.LoadGameScene();
        }

        public void ClickButton()
        {
            SoundManagerOld.instance.SfxPlay("ClickButton", clickAudioClip);
        }

        public void QuitButton()
        {
            SoundManagerOld.instance.SfxPlay("ClickButton", quitAudioClip);
            Application.Quit();
        }
    
        public void BackButton()
        {
            SoundManagerOld.instance.SfxPlay("BackButton", backAudioClip);
        }
    }
}
