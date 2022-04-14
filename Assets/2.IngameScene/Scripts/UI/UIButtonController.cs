using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonController : MonoBehaviour
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
    }


    public void SaveButton()
    {
        SoundManager.instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void SaveAndQuitButton()
    {
        SoundManager.instance.SfxPlay("ClickButton", clickAudioClip);
        // XMLManager.instance.SaveByMXL();
        
        Application.Quit();
    }
}
