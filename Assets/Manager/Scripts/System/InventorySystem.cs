using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("인벤토리 아이템 플레이어 UI에 반영")]
    [SerializeField]
    private GameObject playerUI;
    
    [Header("인벤토리 설정")]
    public GameObject inventorySlotsParent;
    public GameObject inventoryEquipmentSlot;
    
    private Slot[] itemSlots;
    private Slot equipmentSlot;
    private int playerCoinCount;
    
    #region Inventory System 싱글톤 설정
    public static InventorySystem instance; // Game Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Game Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            // 이미 Game Manager가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion

    private void Start()
    {
        // 해당 오브젝트의 자식 slot 오브젝트를 itemSlots 배열에 할당한다.
        itemSlots = inventorySlotsParent.GetComponentsInChildren<Slot>();
        equipmentSlot = inventoryEquipmentSlot.GetComponent<Slot>();
    }

    // 아이템을 획득했을 때 인벤토리에 데이터(아이템)을 저장한다.
    public void AcquireItem(Item item, int count = 1)
    {
        // 획득한 아이템 == 코인
        if (Item.ItemType.Coin == item.itemType)
        {
            playerCoinCount += count;
            PlayerUI getPlayerUI = playerUI.GetComponent<PlayerUI>();
            getPlayerUI.UpdatePlayerCoinCountUI();
            return;
        }
        
        // 장비 아이템이 아닐 경우 수행한다. (장비 아이템은 같은 종류여도 따로 slot에 count를 하도록 구현함.)
        if (Item.ItemType.Equipment != item.itemType)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null) // 아이템 슬롯에 아이템이 있을 때만 수행한다.
                {
                    if (itemSlots[i].item.itemName == item.itemName) // 중복된 아이템이 있을 때
                    {
                        // 아이템의 개수를 증가시켜준다.
                        itemSlots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null) // 아이템 슬롯이 비어있을 때만 수행한다.
            {
                itemSlots[i].AddItem(item, count);
                return;
            }
        }
    }
    
    public void ChangeBgImageAlphaSlots(int alpha)
    {
        Color color;
        
        // 모든 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
        foreach (var slot in itemSlots)
        {
            color = slot.GetComponent<Image>().color;
            color.a = alpha;
            slot.GetComponent<Image>().color = color;
        }
        
        // 장비 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
        color = equipmentSlot.GetComponent<Image>().color;
        color.a = alpha;
        equipmentSlot.GetComponent<Image>().color = color;
    }
    
    public void ChangeBgImageAlphaSlot(int alpha)
    {
    }

    public int GetPlayerCoinCount()
    {
        return playerCoinCount;
    }
}
