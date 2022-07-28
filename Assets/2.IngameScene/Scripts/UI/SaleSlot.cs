using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class SaleSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("장비 슬롯")]
    [SerializeField] private GameObject equipmentSlot;

    [Header("획득한 아이템")]
    public Item item; // 획득한 아이템 DB
    public int itemCount; // 획득한 아이템의 개수
    public int itemSalePrice; 
    public Image itemImage; // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private GameObject item_CountImage; // 획득한 아이템의 배경 이미지
    [SerializeField] private TMP_Text text_Count; // 획득한 아이템의 개수 텍스트
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

    public void InitSetItem(Slot inventorySlot)
    {
        if (inventorySlot.item == null)
        {
            SetItemImageColor(0);
            return;
        }

        item = inventorySlot.item;
        itemCount = inventorySlot.itemCount;
        itemSalePrice = 0;
        itemImage.sprite = inventorySlot.itemImage.sprite;
        
        // 장비 아이템이 아닐 때
        if (item.itemType != Item.ItemType.Equipment)
        {
            text_Count.text = itemCount.ToString();
            item_CountImage.SetActive(true);
        }
        else // 장비 아이템일 때
        {
            text_Count.text = itemCount.ToString("0");
            item_CountImage.SetActive(false);
        }
        SetItemImageColor(1);
    }
    
    // 아이템 개수 조정
    public void SetSlotCount(int count)
    {
        itemCount += count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // UI 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetItemImageColor(0);
        
        text_Count.text = "0";
        item_CountImage.SetActive(false);
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
                if (PlayerEventSystem.instance.NearObject.CompareTag("ShopNpc"))
                {
                    ShopSystem.instance.OpenRequestSaleUI(item);
                }
                else if (PlayerEventSystem.instance.NearObject.CompareTag("MoveShopNPC"))
                {
                    MoveShopSystem.instance.OpenRequestSaleUI(item);
                }

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
