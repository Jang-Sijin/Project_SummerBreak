using System;
using UnityEngine;

[Serializable]
public class SaveInfo
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public int hp;
    public int stamina;
    public string saveTime;

    public SaveInfo()
    {
        
    }
    public SaveInfo(string name, Vector3 position, Vector3 rotation, int hp, int stamina, string saveTime)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.hp = hp;
        this.stamina = stamina;
        this.saveTime = saveTime;
    }
}