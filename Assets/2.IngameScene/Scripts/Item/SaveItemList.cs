using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveItemList : MonoBehaviour
{
    private List<string> allItemNameList = new List<string>();

    
    // 현재 인게임 필드에 있는 획득 가능한 아이템들의 이름을 리스트로 반환한다. 
    public List<string> SaveMapItemList()
    {
        ItemPickUp[] allItemList = gameObject.GetComponentsInChildren<ItemPickUp>();

        foreach (var itemObj in allItemList)
        {
            allItemNameList.Add(itemObj.name);
        }

        return allItemNameList;
    }

    public void LoadMapItemList(List<string> loadItemList)
    {
        ItemPickUp[] allItemList = gameObject.GetComponentsInChildren<ItemPickUp>();

        foreach (var itemObj in allItemList)
        {
            int findItemIndex = loadItemList.FindIndex(item => item == itemObj.gameObject.name);

            if (findItemIndex == -1)
            {
                // 저장된 아이템 이름 리스트에 인게임 필드 아이템의 이름이 없으면 삭제한다. // 이전에 저장한 시점에 이미 획득한 아이템 //
                Destroy(itemObj.gameObject);
            }
        }
    }
}
