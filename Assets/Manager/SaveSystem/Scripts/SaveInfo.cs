using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[Serializable]
//public struct SaveSlotInfo()
//{
//    public string itemName;
//    public int itemCount;
//}

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
    public string equipmentName;
    public int equipmentCount;
    public string[] inventoryItemName;
    public int[] inventoryItemCount;
    public int questProgressID;
    public bool isProgressQuest;
    public bool[] landMarkEnableArray;

    public SaveInfo()
    {
        
    }
    public SaveInfo(string name, string saveTime, Vector3 position, Vector3 rotation, int hp, int maxStamina, int currentStamina, int playerCoinCount, Slot saveEquipmentSlot, Slot[] saveInventorySlots
    , int saveQuestProgressID, bool saveIsProgressQuest, bool[] saveLandMarkEnableArray)
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
        if (saveEquipmentSlot.item)
        {
            this.equipmentName = saveEquipmentSlot.item.itemName;
            this.equipmentCount = saveEquipmentSlot.itemCount;
        }
        this.inventoryItemName = saveInventorySlots.Where(slot => slot.item != null).Select(slot => slot.item.itemName).ToArray();
        this.inventoryItemCount = saveInventorySlots.Where(slot => slot.item != null).Select(slot => slot.itemCount).ToArray();
        
        // 퀘스트
        this.questProgressID = saveQuestProgressID;
        this.isProgressQuest = saveIsProgressQuest;
        
        // 랜드마크
        landMarkEnableArray = saveLandMarkEnableArray;
    }
}