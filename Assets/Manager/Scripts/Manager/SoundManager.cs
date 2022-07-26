using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _soundSource;

    private string _soundManagerPrefabPath = SoundPath.Folder.FolderPath + "/" + "SoundManager";

    public static SoundManager Instance { get; private set; }
    private Dictionary<string, AudioClip> _loadedClip = new Dictionary<string, AudioClip>();

    public void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        SoundManager prefab = Resources.Load<SoundManager>(_soundManagerPrefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[장시진] - Sound Manager Prefab 생성 실패, Prefab Path: {_soundManagerPrefabPath}");
            return;
        }

        Instance = GameObject.Instantiate<SoundManager>(prefab);
        DontDestroyOnLoad(Instance.gameObject);
    }

    public void Start()
    {
        LoadSoundFiles();
    }

    private void LoadSoundFiles()
    {
        // [로딩]
        SoundManager.Instance.LoadBGM(SoundPath.bgmStage1);
        SoundManager.Instance.LoadSound(SoundPath.sfxMouseClick);
    }
    
    private AudioClip LoadAudioClip(string fullPath)
    {
        AudioClip clip = null;
        if (_loadedClip.TryGetValue(fullPath, out clip))
        {
            return clip;
        }

        clip = Resources.Load<AudioClip>(fullPath);
        if (clip == null)
        {
            Debug.LogError("[SoundManager.LoadAudioClip.InvalidPath]" + fullPath);
            return null;
        }

        _loadedClip.Add(fullPath, clip);
        return clip;
    }

    private static string GetBGMFullPath(string path)
    {
        return SoundPath.Folder.FolderPath + "/" + path;
    }

    public void LoadBGM(string path)
    {
        LoadAudioClip(GetBGMFullPath(path));
    }

    public void PlayBGM(string path)
    {
        AudioClip clip = LoadAudioClip(GetBGMFullPath(path));
        if (clip == null)
        {
            return;
        }
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    private static string GetSoundFullPath(string path)
    {
        return SoundPath.Folder.FolderPath + "/" + path;
    }

    public void LoadSound(string path)
    {
        LoadAudioClip(GetSoundFullPath(path));
    }

    public void PlaySound(string path)
    {
        AudioClip clip = LoadAudioClip(GetSoundFullPath(path));
        if (clip == null)
        {
            return;
        }
        _soundSource.PlayOneShot(clip);
    }

    public void ClearLoadedAudioClip()
    {
        _loadedClip.Clear();
    }
}
