using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LankMarkSystem : MonoBehaviour
{

    [SerializeField] 
    private List<GameObject> landmarkOBJ = new List<GameObject>();

    [SerializeField] private GameObject landMarkCutScene01;
    [SerializeField] private GameObject landMarkCutScene02;
    [SerializeField] private GameObject landMarkCutScene03;
    [SerializeField] private GameObject landMarkCutScene04;
    [SerializeField] private GameObject landMarkCutScene05;

    [SerializeField] 
    private List<bool> landmarkDialogs = new List<bool>();

    #region Singleton

    public static LankMarkSystem instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion



    public void StartLandMarkDialog()
    {
        MapOpenTrigger mapOpenTrigger = PlayerEventSystem.instance.NearObject.GetComponent<MapOpenTrigger>();
        landmarkDialogs[mapOpenTrigger.landMarkNumber - 1] = true;
    }

    public void EndLandMarkDialog()
    {
        MapOpenTrigger mapOpenTrigger = PlayerEventSystem.instance.NearObject.GetComponent<MapOpenTrigger>();
        landmarkDialogs[mapOpenTrigger.landMarkNumber - 1] = false;
    }
    
    
    
    public bool GetNumberDialogsEnable(int number)
    {
        if (landmarkDialogs[number])
        {
            return true;
        }
        
        return false;
    }
    
}
