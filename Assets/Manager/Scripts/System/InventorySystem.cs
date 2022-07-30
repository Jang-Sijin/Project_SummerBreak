using System;
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
        
        // [임시 빌드용 디폴트 아이템] // 나중에 제대로 함수 추가 필요
        // itemSlots[9].AddItem(sword);
        // itemSlots[10].AddItem(bag);
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

    public void LoadInventoryCoin(int coinCount)
    {
        playerCoinCount = coinCount;
        PlayerUI getPlayerUI = playerUI.GetComponent<PlayerUI>();
        getPlayerUI.UpdatePlayerCoinCountUI();

        return;
    }

    public void CheckSlotList()
    {
        // 인벤토리 슬롯들과 장비 슬롯에 어떤 아이템이 들어가 있는지 확인한다. //
        for (int i = 0; i < itemSlots.Length; i++)
        {
            
        }
    }

    public void ExportInventorySlotsData(Slot[] slots)
    {
        if (slots.Length != itemSlots.Length)
        {
            print($"[장시진] 상점 인벤토리 크기와 플레이어 인벤토리 크기가 서로 다릅니다. ExportInventorySlotsData Func Err");
            return;
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            slots[i] = itemSlots[i];
        }
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
    
    // findItemName과 같은 아이템이 인벤토리에 존재하는지 확인한다. [아이템의 개수를 반환합니다. // 아이템이 없으면 0을 반환]
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
}
