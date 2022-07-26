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
        SoundManagerOld.instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void BackButton()
    {
        SoundManagerOld.instance.SfxPlay("BackButton", backAudioClip);
    }

    public void QuitButton()
    {
        SoundManagerOld.instance.SfxPlay("ClickButton", quitAudioClip);
    }


    public void SaveButton()
    {
        SoundManagerOld.instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void SaveAndQuitButton()
    {
        SoundManagerOld.instance.SfxPlay("ClickButton", clickAudioClip);
        // XMLManager.instance.SaveByMXL();
        
        Application.Quit();
    }
}
