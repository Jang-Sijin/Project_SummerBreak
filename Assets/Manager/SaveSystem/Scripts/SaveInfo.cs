using System;
using UnityEngine;

[Serializable]
public class SaveInfo
{
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public int hp;
    public int maxStamina;
    public int currentStamina;
    public int playerCoinCount;
    public string saveTime;

    public SaveInfo()
    {
        
    }
    public SaveInfo(string name, Vector3 position, Vector3 rotation, int hp, int maxStamina, int currentStamina, int playerCoinCount, string saveTime)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.hp = hp;
        this.maxStamina = maxStamina;
        this.currentStamina = currentStamina;
        this.playerCoinCount = playerCoinCount;
        this.saveTime = saveTime;
    }
}