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
        SoundManager.Instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void BackButton()
    {
        SoundManager.Instance.SfxPlay("BackButton", backAudioClip);
    }

    public void QuitButton()
    {
        SoundManager.Instance.SfxPlay("ClickButton", quitAudioClip);
    }


    public void SaveButton()
    {
        SoundManager.Instance.SfxPlay("ClickButton", clickAudioClip);
    }
    
    public void SaveAndQuitButton()
    {
        SoundManager.Instance.SfxPlay("ClickButton", clickAudioClip);
        // XMLManager.Instance.SaveByMXL();
        
        Application.Quit();
    }
}
