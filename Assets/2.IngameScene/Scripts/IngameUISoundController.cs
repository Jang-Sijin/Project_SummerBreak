using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUISoundController : MonoBehaviour
{
    public AudioClip clickAudioClip;
    public AudioClip quitAudioClip;
    public AudioClip backAudioClip;

    private void Start()
    {
        
    }

    public void ClickButton()
    {
        SoundManager.instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void BackButton()
    {
        SoundManager.instance.SfxPlay("BackButton", backAudioClip);
    }

    public void QuitButton()
    {
        SoundManager.instance.SfxPlay("ClickButton", quitAudioClip);
        Application.Quit();
    }
}
