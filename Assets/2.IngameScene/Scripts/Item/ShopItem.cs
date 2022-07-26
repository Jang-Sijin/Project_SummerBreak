using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShopItem", menuName = "New ShopItem/item")] // 마우스 우클릭 + Create에서 추가할 수 있습니다.
[Serializable]
public class ShopItem : ScriptableObject
{
    public int itemCount;
    public Item item;
    public int buyItemPrice;
    public int saleItemPrice;
}

// MonoBehaviour: 유니티 게임오브젝트 컴포넌트에 스크립터를 부착해야 해당 스크립터의 효력이 생긴다.
// ScriptableObject: 굳이 게임오브젝트 컴포넌트에 부착할 필요가 없다.