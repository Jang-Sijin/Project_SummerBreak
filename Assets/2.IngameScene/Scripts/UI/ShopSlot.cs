using KON;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("마우스 위치")]
    private Vector3 _originMousePos;
    
    [Header("슬롯에 들어오는 아이템")]
    public Item item; // 획득한 아이템 DB
    public int itemCount; // 획득한 아이템의 개수
    public int itemBuyPrice; // 아이템의 가격
    public int itemSalePrice; // 아이템의 가격
    public Image itemImage; // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private TMP_Text itemCountText; // 상점에서 판매중인 아이템의 개수 텍스트
    [SerializeField] private TMP_Text itemPriceText; // 상점에서 판매중인 아이템의 가격 텍스트
    
    [Header("아래 TMP_Text: Inventory 하위 오브젝트에 있는 이름과 동일한 오브젝트를 연결해 주세요.")]
    [SerializeField] private TMP_Text item_ShowName_Text; // 아이템의 이름(정보)을 보여주는 텍스트
    [SerializeField] private TMP_Text item_ShowInfo_Text; // 아이템의 내용(정보)를 보여주는 텍스트
    
    // 마우스 좌버튼 클릭 체크 (아이템 선택 유/무)
    private bool isMouseLeftClick;

    // 이미지의 투명도 조절
    private void SetItemImageColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void SetItem(ShopItem _shopItem)
    {
        item = _shopItem.item;
        itemCount = _shopItem.itemCount;
        itemBuyPrice = _shopItem.buyItemPrice;
        itemSalePrice = _shopItem.saleItemPrice;
        itemImage.sprite = _shopItem.item.itemImage;
        
        itemCountText.text = _shopItem.itemCount.ToString();
        itemPriceText.text = $"{_shopItem.buyItemPrice}C";

        SetItemImageColor(1);
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        itemCountText.text = itemCount.ToString();
    }

    // 마우스 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[장시진] ShopItemSlot OnPointerClick 호출");

        // 스크립터를 부착한 오브젝트에 마우스 우클릭을 하였을 때 이벤트를 실행
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null && isMouseLeftClick)
            {
                ShopSystem.instance.OpenRequestBuyUI(item);
                print($"{isMouseLeftClick}");
            }
            else
            {
                print($"{isMouseLeftClick}");
            }
            isMouseLeftClick = false;
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                isMouseLeftClick = true;
                print($"{isMouseLeftClick}");
            }
            else
            {
                isMouseLeftClick = false;
                print($"{isMouseLeftClick}");
            }
        }
    }
}
