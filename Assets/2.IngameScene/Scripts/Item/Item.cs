using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")] // 마우스 우클릭 + Create에서 추가할 수 있습니다.
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,      // 장비
        Consumables,    // 소비
        Etc             // 기타
    }
    
    public string itemName;         // 아이템 이름
    [TextArea]
    public string itemInfo;         // 아이템 정보(내용)
    public Sprite itemImage;        // 아이템 이미지(UI)
    public GameObject itemPrefab;   // 아이템 프리팹
    public ItemType itemType;       // 아이템 타입
}

// MonoBehaviour: 유니티 게임오브젝트 컴포넌트에 스크립터를 부착해야 해당 스크립터의 효력이 생긴다.
// ScriptableObject: 굳이 게임오브젝트 컴포넌트에 부착할 필요가 없다.
