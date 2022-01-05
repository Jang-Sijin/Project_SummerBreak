using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMenuController : MonoBehaviour
{
    public AudioClip startAudioClip;
    public AudioClip loadStartAudioClip;
    public AudioClip optionAudioClip;
    public AudioClip quitAudioClip;
    public AudioClip backAudioClip;

    public void StartButton()
    {
        SoundManager.instance.SfxPlay("StartButton", startAudioClip);
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
