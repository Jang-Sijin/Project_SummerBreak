using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("인벤토리 설정")]
    public GameObject inventorySlotsParent;
    private Slot[] slots;
    
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
        // 해당 오브젝트의 자식 slot 오브젝트를 slots 배열에 할당한다.
        slots = inventorySlotsParent.GetComponentsInChildren<Slot>();
    }

    // 아이템을 획득했을 때 인벤토리에 데이터(아이템)을 저장한다.
    public void AcquireItem(Item item, int count = 1)
    {
        // 장비 아이템이 아닐 경우 수행한다. (장비 아이템은 같은 종류여도 따로 slot에 count를 하도록 구현함.)
        if (Item.ItemType.Equipment != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) // 아이템 슬롯에 아이템이 있을 때만 수행한다.
                {
                    if (slots[i].item.itemName == item.itemName) // 중복된 아이템이 있을 때
                    {
                        // 아이템의 개수를 증가시켜준다.
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // 아이템 슬롯이 비어있을 때만 수행한다.
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
    }
}
