using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveInfo
{
    public string name;
    public string saveTime;
    public Vector3 position;
    public Vector3 rotation;
    public int hp;
    public int maxStamina;
    public int currentStamina;
    public int playerCoinCount;
    public Dictionary<string, int> equipmentItem;
    public Dictionary<string, int> inventoryItem;
    public int questProgressID;
    public bool isProgressQuest;
    public bool[] landMarkEnableArray;
    public List<string> fieldItemList;
    public Dictionary<string, bool> fieldChestBoxList;

    public SaveInfo()
    {
        
    }
    
    public SaveInfo(string name, string saveTime, Vector3 position, Vector3 rotation, int hp, int maxStamina, int currentStamina, int playerCoinCount, Slot saveEquipmentSlot, Slot[] saveInventorySlots
    , int saveQuestProgressID, bool saveIsProgressQuest, bool[] saveLandMarkEnableArray, List<string> saveItemList, Dictionary<string, bool> saveChestBoxList)
    {
        this.name = name;
        this.saveTime = saveTime;
        this.position = position;
        this.rotation = rotation;
        this.hp = hp;
        this.maxStamina = maxStamina;
        this.currentStamina = currentStamina;
        this.playerCoinCount = playerCoinCount;
        
        // 인벤토리
        equipmentItem= new Dictionary<string, int>();
        inventoryItem = new Dictionary<string, int>();
        if (saveEquipmentSlot != null && saveEquipmentSlot.item != null)
        {
            Debug.Log(saveEquipmentSlot.item.name);
            equipmentItem.Add(saveEquipmentSlot.item.name, saveEquipmentSlot.itemCount);
        }
        else
        {
            equipmentItem.Add("null", 0);
        }
        
        if (saveInventorySlots != null && saveInventorySlots.Length != 0)
        {
            foreach (var itemSlot in saveInventorySlots)
            {
                if (itemSlot.item != null)
                {
                    Debug.Log(itemSlot.item.name);
                    inventoryItem.Add(itemSlot.item.name, itemSlot.itemCount);
                }
            }
        }

        // 퀘스트
        this.questProgressID = saveQuestProgressID;
        this.isProgressQuest = saveIsProgressQuest;
        
        // 랜드마크
        landMarkEnableArray = saveLandMarkEnableArray;
        
        // 필드 아이템 리스트
        fieldItemList = saveItemList;
        
        // 상자 Open/Close 리스트
        fieldChestBoxList = saveChestBoxList;
    }
}