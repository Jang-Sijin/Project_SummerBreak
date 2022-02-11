using System;
using UnityEngine;

public class SavePlayerInfo
{
    // public int saveSlotNum;
    // public string playTime;
    // public string playerName;
    // public string mapLocation;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    // public int hp;
    // public int stamina;
}

public class PlayerInfo : MonoBehaviour
{

    private void Awake()
    {
    }

    private void Start()
    {
        this.gameObject.transform.position = GameManager.instance.playerGameObject.transform.position;
        this.gameObject.transform.rotation = GameManager.instance.playerGameObject.transform.rotation;
        
        Debug.Log($"{this.gameObject.transform.position}");
        Debug.Log($"{this.gameObject.transform.rotation}");
    }
}