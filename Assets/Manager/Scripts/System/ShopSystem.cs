using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    // [프리팹 셋팅]
    [Header("상점 캔버스")]
    [SerializeField] private GameObject shopCanvas;
    
    [Header("구매 UI")]
    [SerializeField] private GameObject shopBuyUI;
    
    [Header("판매 UI")]
    [SerializeField] private GameObject shopSaleUI;
    
    [Header("상점 슬롯 설정")]
    [SerializeField] private GameObject shopSlotsParent;
    
    [Header("인벤토리 슬롯 설정")]
    [SerializeField] private GameObject inventoryEquipmentSlot;
    [SerializeField] private GameObject inventorySlotsParent;

    [Header("상점 버튼 설정")]
    [SerializeField] private GameObject shopBuyButton;
    [SerializeField] private GameObject shopSaleButton;
    
    [Header("상점 뒤로가기 버튼 설정")]
    [SerializeField] private GameObject shopBuyBackButton;
    [SerializeField] private GameObject shopSaleBackButton;
    
    [Header("상점 구매 확인 UI 설정")]
    [SerializeField] private GameObject shopBuyRequestUI;
    [SerializeField] private GameObject shopBuyRequestYesButton;
    [SerializeField] private GameObject shopBuyRequestNoButton;
    [SerializeField] private GameObject shopBuyRequestFailUI;
    
    [Header("상점 아이템 슬롯 리스트")] 
    [SerializeField] private ShopItem[] shopItemList;
    [Header("인벤토리 슬롯 리스트")] 
    [SerializeField] private Slot[] inventorySlotList;
    
    // [로컬 변수]
    // 상점 아이템 리스트들을 슬롯에 할당한다.
    private ShopSlot[] shopItemSlots;
    // 상점 아이템(슬롯)을 마우스로 선택하면 상점 아이템 리스트 배열의 어느 아이템인지 확인하는 변수
    private ShopItem selectShopItem;
    // 인벤토리 아이템 리스트들을 슬롯에 할당한다.
    private Slot[] itemSlots;

    // 상점 구매/판매 버튼 이미지
    private Image shopBuyButtonImage;
    private Image shopSaleButtonImage;
    
    #region Shop System 싱글톤 설정
    public static ShopSystem instance; // Game Manager을 싱글톤으로 관리
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
        shopItemSlots = shopSlotsParent.GetComponentsInChildren<ShopSlot>();
        inventorySlotList = inventorySlotsParent.GetComponentsInChildren<Slot>();
        
        Init();
    }

    private void Init()
    {
        InitShopItemList();
        InitSetButton();
        InitSetUI();
    }

    private void InitShopItemList()
    {
        try
        {
            for (int i = 0; i < shopItemSlots.Length; i++)
            {
                if (shopItemSlots[i].item == null) // 아이템 슬롯이 비어있을 때만 수행한다.
                {
                    shopItemSlots[i].SetItem(shopItemList[i]);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ShopSystem InitShopItemList ["+ e + "] 오류가 발생하였습니다.");
            throw;
        }
    }
    
    private void InitSetButton()
    {
        shopBuyButtonImage = shopBuyButton.GetComponent<Image>();
        shopSaleButtonImage = shopSaleButton.GetComponent<Image>();

        // 상점을 켰을 때 구매창이 먼저 활성화 된다.
        shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }

    private void InitSetUI()
    {
        shopBuyRequestUI.SetActive(false);
    }
    
    public void ChangeBgImageAlphaSlots(int alpha)
    {
        // 모든 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
        foreach (var slot in shopItemSlots)
        {
            Color color = slot.GetComponent<Image>().color;
            color.a = alpha;
            slot.GetComponent<Image>().color = color;
        }
    }

    public void ClickShopBuyButton()
    {
        // 상점을 켰을 때 구매 버튼을 눌렀을 때
        shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }
    
    public void ClickShopSaleButton()
    {
        // 상점을 켰을 때 구매 버튼을 눌렀을 때
        shopBuyButtonImage.color = new Color(1, 1, 1, 0);
        shopSaleButtonImage.color = new Color(1, 1, 1, 1);
    }

    public void ClickCloseShopButton()
    {
        // 상점 시스템을 종료할 때 상점 구매 목록이 보이도록 설정 후 종료한다. 
        shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        shopSaleButtonImage.color = new Color(1, 1, 1, 0);
        
        // 상점 인벤토리 슬롯의 내용을 인벤토리 시스템의 아이템 슬롯에 저장(반영)한다
        // ...
    }

    public void ClickRequestYesButton()
    {
        // [상점에서 선택한 슬롯의 아이템을 구매한다.] // 선택지: [구매성공/ 구매실패]
        // 1. 선택한 상점 슬롯의 아이템을 인벤토리에 1개 추가한다.
        // 2. 플레이어가 소지하고 있는 코인에 선택한 상점 슬롯의 아이템의 가격(코인)을 감소시킨다.
        // selectShopItem
        if (selectShopItem != null)
        {
            // 1.상점에서 판매하는 아이템이 0보다 클 경우(구매할 수 있는 조건) + 2.플레이어가 소지하고 있는 코인이 선택한 상점 슬롯의 아이템의 가격(코인)보다 크거나 같을 때 (구매할 수 있는 조건)
            if ((selectShopItem.itemCount > 0) && (InventorySystem.instance.playerCoinCount >= selectShopItem.itemPrice))
            {
                // 해당 아이템을 인벤토리에 추가한다. (1개 구매)
                InventorySystem.instance.AcquireItem(selectShopItem.item, 1);
                
                // 플레이어가 소지중인 코인의 개수를 아이템의 가격만큼 감소한다.
                InventorySystem.instance.playerCoinCount -= selectShopItem.itemPrice;

                // 플레이어 UI (코인의 개수 출력 이미지 텍스트)를 갱신한다. 
                PlayerUI playerUI = GameManager.instance.PlayerUI.GetComponent<PlayerUI>();
                playerUI.UpdatePlayerCoinCountUI();

                // 활성화된 RequestUI를 비활성화 한다.
                shopBuyRequestUI.SetActive(false);
            }
            else
            {
                // 1.아이템 개수 소진으로 아이템 구매 불가능. or 2.플레이어 소지 코인의 개수가 부족하여 아이템 구매 불가능.
                
                // 활성화된 RequestUI를 비활성화 한다.
                shopBuyRequestUI.SetActive(false);
                
                // 활성화된 RequestFailUI를 활성화 한다.
                shopBuyRequestFailUI.SetActive(true);
            }
        }
        else
        {
            print($"[장시진] 상점에서 선택한 슬롯이 존재하지 않습니다. [오류]");
            shopBuyRequestUI.SetActive(false);
        }
    }

    public void ClickRequestNoButton()
    {
        // 상점에서 선택한 슬롯의 정보를 초기화한다. 
        selectShopItem = null;
        
        // 활성화된 RequestUI를 비활성화 한다.
        shopBuyRequestUI.SetActive(false);
    }

    public void OpenRequestBuyUI(Item slotItem)
    {
        shopBuyRequestUI.SetActive(true);

        for (int i = 0; i < shopItemList.Length; ++i)
        {
            // 상점 아이템 DB의 item과 마우스로 선택한 아이템 슬롯의 DB가 같으면 
            if (shopItemList[i].item == slotItem)
            {
                // 현재 선택한 슬롯의 아이템을 selectShopItem 변수에 담는다.
                selectShopItem = shopItemList[i];

                return;
            }
        }
    }

    public void OpenShopCanvas()
    {
        shopCanvas.gameObject.SetActive(true);
    }
}
