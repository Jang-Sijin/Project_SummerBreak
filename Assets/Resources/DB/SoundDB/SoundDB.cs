using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SoundDB", menuName = "New SoundDB/Sound")] // 마우스 우클릭 + Create에서 추가할 수 있습니다.
public class SoundDB : ScriptableObject
{
    public string[] soundName;
    public AudioClip[] soundAudioClip;
}