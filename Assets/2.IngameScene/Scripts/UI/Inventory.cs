using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리 시스템이 활성화 체크
    [SerializeField] private GameObject go_InventoryBase;

    [SerializeField] private GameObject go_SlotsParent;

    private Slot[] slots;

    private void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); // 해당 오브젝트의 자식 slot 오브젝트를 slots 배열에 할당한다.
    }

    // private void TryOpenInventory()
    // {
    //     if (Input.GetKeyDown(KeyCode.I))
    //     {
    //         inventoryActivated = !inventoryActivated;
    //
    //         if (inventoryActivated)
    //         {
    //             OpenInventory();
    //         }
    //         else
    //         {
    //             CloseInventory();
    //         }
    //             
    //     }
    // }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
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

    public void ChangeBgImageAlphaSlots(int alpha)
    {
        // 모든 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
        foreach (var slot in slots)
        {
            Color color = slot.GetComponent<Image>().color;
            color.a = alpha;
            slot.GetComponent<Image>().color = color;
        }
    }
}
