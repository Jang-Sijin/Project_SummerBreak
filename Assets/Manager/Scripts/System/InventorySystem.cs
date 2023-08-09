using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("인벤토리 아이템 플레이어 UI에 반영")]
    [SerializeField]
    private GameObject playerUI;
    
    [Header("인벤토리 설정")]
    [SerializeField] private GameObject inventorySlotsParent;
    [SerializeField] private GameObject inventoryEquipmentSlot;

    [Header("테스트 모드 인벤토리 설정")]
    [SerializeField] private Item pen;

    private Slot[] itemSlots;
    private Slot equipmentSlot;

    [Header("플레이어가 획득한 코인 개수")]
    public int playerCoinCount;
    
    [Header("아이템 DB 리스트")]
    [SerializeField] private ItemList ItemDB;

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
        if (JsonManager.instance.CheckSaveFile())
            return;
        
        // 해당 오브젝트의 자식 slot 오브젝트를 itemSlots 배열에 할당한다.
        itemSlots = inventorySlotsParent.GetComponentsInChildren<Slot>();
        equipmentSlot = inventoryEquipmentSlot.GetComponent<Slot>();

        // [임시 빌드용 디폴트 아이템] //
        itemSlots[11].AddItem(pen);
    }

    // parameter1이 양수면 코인 개수 +, 음수면 코인 개수 - 
    public void SetPlayerCoinCount(int count)
    {
        playerCoinCount += count;
        
        // UI 갱신
        PlayerUI getPlayerUI = playerUI.GetComponent<PlayerUI>();
        getPlayerUI.UpdatePlayerCoinCountUI();
        return;
    }

    // 아이템을 획득했을 때 인벤토리에 데이터(아이템)을 저장한다.
    public void AcquireItem(Item item, int count = 1)
    {
        // 획득한 아이템 == 코인
        if (Item.ItemType.Coin == item.itemType)
        {
            if (item.itemName == "1코인")
            {
                playerCoinCount += count;
            }
            else if (item.itemName == "5코인")
            {
                playerCoinCount += (count * 5);
            }
            else if (item.itemName == "10코인")
            {
                playerCoinCount += (count * 10);
            }
              
            // 코인 소유 개수 출력 UI(Text) 갱신
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
    
    // 아이템 선택 시 테두리 애니메이션, 배경 알파값 조정
    public void ChangeBgImageAlphaSlots(int alpha)
    {
        Color color;
        
        // 모든 아이템 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
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

    // 인게임 접속 시 인벤토리 아이템 셋팅
    public void LoadInventory(int coinCount, Dictionary<string, int> getEquipment, Dictionary<string, int> getInventorySlotItems)
    {
        // 코인(재화) 셋팅
        playerCoinCount = coinCount;
        PlayerUI getPlayerUI = playerUI.GetComponent<PlayerUI>();
        // 코인(재화)개수 및 UI 업데이트
        getPlayerUI.UpdatePlayerCoinCountUI();
        
        
        // 해당 오브젝트의 자식 slot 오브젝트를 itemSlots 배열에 할당한다.
        itemSlots = inventorySlotsParent.GetComponentsInChildren<Slot>();
        equipmentSlot = inventoryEquipmentSlot.GetComponent<Slot>();
        
        
        // 이전 인벤토리 장비 슬롯 셋팅
        if (getEquipment != null)
        {
            foreach (var equipment in getEquipment)
            {
                var findItem = Array.Find(ItemDB.ItemDBList, item => item.name == equipment.Key);

                if (findItem != null)
                {
                    equipmentSlot.AddItem(findItem, equipment.Value);
                    break;
                }
            }
        }

        int i = 0;
        // 이전 인벤토리 아이템 슬롯 셋팅
        foreach (var getItemIndex in getInventorySlotItems)
        {
            Item findItem = Array.Find(ItemDB.ItemDBList, item => item.name == getItemIndex.Key);

            if (findItem != null)
            {
                itemSlots[i++].AddItem(findItem, getItemIndex.Value);
            }
        }

        return;
    }

    // 인벤토리에 있는 아이템의 정보들을 상점 판매 UI 리스트(슬롯)에 초기화 합니다.
    public void InitShopSaleInventorySlots(ref SaleSlot saleEquipmentSlot, ref SaleSlot[] saleSlots)
    {
        // 판매 아이템 리스트 셋팅 - 장비창 
        if (equipmentSlot != null)
        {
            saleEquipmentSlot.InitSetItem(equipmentSlot);
        }

        // 판매 아이템 리스트 셋팅 - 일반 슬롯
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item != null)
            {
                saleSlots[i].InitSetItem(itemSlots[i]);
            }
        }
    }
    
    // parameter1의 아이템 이름으로 인벤토리 슬롯을 검색하여 획득한 아이템을 찾아서 parameter2 만큼 아이템 개수를 변경한다.
    // 아이템 개수 증가/감소가 필요할 때 사용하시면 됩니다.
    public void FindSetCountInventorySlotItem(string itemName, int count = -1)
    {
        if (equipmentSlot.item != null && equipmentSlot.item.itemName == itemName)
        {
            equipmentSlot.SetSlotCount(count);
            return;
        }

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.item != null && itemSlot.item.itemName == itemName)
            {
                itemSlot.SetSlotCount(count);
                break;
            }
        }
        return;
    }
    
    // findItemName과 같은 아이템이 인벤토리에 존재하는지 확인한다.
    // [아이템의 개수를 반환합니다. // 아이템이 없으면 0을 반환]
    public int FindInventorySlotItem(string findItemName)
    {
        if (equipmentSlot.item != null && equipmentSlot.item.itemName == findItemName)
        {
            return equipmentSlot.itemCount;
        }

        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.item != null && itemSlot.item.itemName == findItemName)
            {
                return itemSlot.itemCount;
            }
        }

        return 0;
    }

    // 인벤토리 장비 슬롯 데이터 가져오기
    public Slot GetEquipmentSlot()
    {
        return equipmentSlot;
    }
    
    // 인벤토리 아이템 슬롯 데이터 가져오기
    public Slot[] GetInventoryItems()
    {
        return itemSlots;
    }
}
