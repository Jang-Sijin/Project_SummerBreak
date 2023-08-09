using KON;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("장비 슬롯")]
    [SerializeField] private GameObject equipmentSlot;
    
    [Header("마우스 위치")]
    private Vector3 originMousePos;
    
    [Header("획득한 아이템")]
    public Item item; // 획득한 아이템 DB
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    // 필요한 컴포넌트
    [SerializeField] private GameObject itemCountImage; // 획득한 아이템의 배경 이미지
    [SerializeField] private TMP_Text textCount; // 획득한 아이템의 개수 텍스트
    [Header("아래 TMP_Text: Inventory 하위 오브젝트에 있는 이름과 동일한 오브젝트를 연결해 주세요.")]
    [SerializeField] private TMP_Text itemShowNameText; // 아이템의 이름(정보)을 보여주는 텍스트
    [SerializeField] private TMP_Text itemShowInfoText; // 아이템의 내용(정보)를 보여주는 텍스트

    private void Start()
    {
        originMousePos = this.transform.position;
    }

    // 이미지의 투명도 조절
    private void SetItemImageColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = _item.itemImage;

        // 장비 아이템이 아닐 때
        if (item.itemType != Item.ItemType.Equipment)
        {
            textCount.text = itemCount.ToString();
            itemCountImage.SetActive(true);
        }
        else // 장비 아이템일 때
        {
            textCount.text = itemCount.ToString("0");
            itemCountImage.SetActive(false);
        }

        SetItemImageColor(1);
    }

    // 아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

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
        
        textCount.text = "0";
        itemCountImage.SetActive(false);
    }

    // 마우스 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[장시진] ItemSlot OnPointerClick 호출");

        // 스크립터를 부착한 오브젝트에 마우스 우클릭을 하였을 때 이벤트를 실행
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Item.ItemType.Equipment) // 장비 아이템일 때
                {
                    // 장비 아이템은 장착은 하지만
                    // 장비를 사용하거나 소모되는 규칙은 없습니다.
                    // 기획상 장비 아이템을 장착 여부만 필요합니다.
                }
                else if(item.itemType == Item.ItemType.Consumables) // 소모 아이템
                {
                    PlayerStatus playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
                     switch (item.itemName)
                    {
                         case "싱글하트물약":
                            playerStatus.HealHealth(20.0f);
                            break;
                         case "더블하트물약":
                             playerStatus.HealHealth(40.0f);
                             break;
                         case "태양석":
                             playerStatus.HealMaxStamina();
                             break;
                    }
                    // 소모
                    Debug.Log($"[장시진] 소모 아이템 {item.itemName}을 사용.");
                    SetSlotCount(-1);
                }
                else // 기타 아이템 ETC
                {
                    Debug.Log($"[장시진] 기타 아이템 {item.itemName}을 사용.");
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null && item.itemInfo != null)
            {
                Debug.Log($"{item.itemInfo}");
                itemShowNameText.text = item.itemName;
                itemShowInfoText.text = item.itemInfo;
            }
        }
    }

    // 마우스 드래그가 시작되었을 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("[장시진] ItemSlot OnBeginDrag 호출");
        
        // 아이템이 있을 때 드래그의 위치를 슬롯의 위치로 설정해준다.
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그 중일 때
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("[장시진] ItemSlot OnDrag 호출");
        
        // 아이템이 있을 때 드래그의 위치를 슬롯의 위치로 설정해줍니다.
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    // 마우스 드래그가 끝났을 때
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("[장시진] ItemSlot OnEndDrag 호출");
        
        DragSlot.instance.SetAlphaColor(0);
        DragSlot.instance.dragSlot = null;
    }

    // 각 슬롯 간의 아이템 교체 // 드래그를 멈춘 위치에 있는 오브젝트에서 호출됩니다.
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("[장시진] ItemSlot OnDrop 호출");
        
        if (DragSlot.instance.dragSlot != null)
        {
            // 마우스 드래그를 통한 슬롯 교환
            ChangeSlot();
        }
    }

    // 마우스 드래그를 통한 슬롯 교환
    private void ChangeSlot()
    {
        // 아이템과 아이템의 개수를 복사한다.
        Item copyItem = item;
        int copyItemCount = itemCount;

        Slot CheckEqipmentSlot = equipmentSlot.GetComponent<Slot>();
        // 1.현재 슬롯이 장비 슬롯이면서 2.드래그 중인 아이템의 타입이 장비 아이템일 경우
        if (this == CheckEqipmentSlot && item == null && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("[장시진] 이동 가능한 슬롯 위치가 아닙니다. 기존 슬롯으로 다시 이동합니다.");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("[장시진] 비어있는 장비슬롯에 장비 아이템 장착");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if (this == CheckEqipmentSlot && item != null && item.itemType == Item.ItemType.Equipment && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("[장시진] 비어있지 않은 장비슬롯에 장비 아이템 장착");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("[장시진] 예외");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if (this != CheckEqipmentSlot && item != null && item.itemType == Item.ItemType.Equipment && DragSlot.instance.dragSlot.item != null)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("장비 아이템이 있는 일반슬롯과 드래그 중인 아이템과 교환");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("장비 copyItem null");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if (this != CheckEqipmentSlot && item != null && item.itemType != Item.ItemType.Equipment && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Equipment)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("일반 아이템이 있는 일반슬롯과 드래그 중인 일반 아이템과 교환");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("장비 copyItem null");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if (this != CheckEqipmentSlot && item != null && item.itemType != Item.ItemType.Equipment && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment && 
                 CheckEqipmentSlot == null)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("일반 아이템이 있는 일반슬롯과 장비 슬롯이 아닌 장비 아이템과 교환");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("장비 copyItem null");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if (this != CheckEqipmentSlot && item != null && item.itemType != Item.ItemType.Equipment && DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment && 
                 CheckEqipmentSlot != null && CheckEqipmentSlot.item != DragSlot.instance.dragSlot.item)
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("일반 아이템이 있는 일반슬롯과 장비 슬롯이 아닌 장비 아이템과 교환");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("장비 copyItem null");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
        else if(this != CheckEqipmentSlot && item == null) // 장비 슬롯x && 슬롯에 아이템x
        {
            AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
            
            if (copyItem != null)
            {
                Debug.Log("장비 아이템 교환");
                DragSlot.instance.dragSlot.AddItem(copyItem, copyItemCount);
            }
            else
            {
                Debug.Log("비어있는 슬롯과 아이템 교환");
                DragSlot.instance.dragSlot.ClearSlot();
            }
        }
    }

    private void OnDisable()
    {
        Color color = new Color(1, 1, 1, 0);
        this.GetComponent<Image>().color = color;
    }
}
