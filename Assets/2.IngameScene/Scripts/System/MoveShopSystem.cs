using System;
using UnityEngine;
using UnityEngine.UI;

public class MoveShopSystem : MonoBehaviour
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

    [Header("상점 판매 인벤토리 슬롯 설정")] 
    [SerializeField] private GameObject shopInventoryEquipmentSlot;
    [SerializeField] private GameObject shopInventorySlotsParent;

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

    [Header("상점 판매 확인 UI 설정")] 
    [SerializeField] private GameObject shopSaleRequestUI;
    
    [SerializeField] 
    private GameObject shopSaleRequestFailUI;

    [Header("플레이어 코인 UI 설정")] 
    [SerializeField] private GameObject playerCoinUI;

    [Header("이동상점 구매/판매 아이템DB 리스트")] 
    [SerializeField] private ShopItem[] shopItemList;

    // [상점 구매UI]
    // 상점 아이템 리스트들을 슬롯에 할당한다.
    private ShopSlot[] shopItemSlots;

    // 상점 아이템(슬롯)을 마우스로 선택하면 상점 아이템 리스트 배열의 어느 아이템인지 확인하는 변수
    private ShopItem selectBuyShopItem;

    // [상점 판매UI]
    private SaleSlot saleInventoryEquipmentSlot;

    private SaleSlot[] saleInventorySlots;

    // 상점 아이템(슬롯)을 마우스로 선택하면 상점 아이템 리스트 배열의 어느 아이템인지 확인하는 변수
    private SaleSlot selectSaleShopItem; // 마우스로 클릭하여 선택한 SaleSlot의 ref(원본)을 가져온다.

    // [상점 구매/판매 버튼 이미지]
    private Image shopBuyButtonImage;
    private Image shopSaleButtonImage;

    #region Move Shop System 싱글톤 설정

    public static MoveShopSystem instance; // Game Manager을 싱글톤으로 관리

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

        saleInventoryEquipmentSlot = shopInventoryEquipmentSlot.GetComponent<SaleSlot>();
        saleInventorySlots = shopInventorySlotsParent.GetComponentsInChildren<SaleSlot>();

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
            Console.WriteLine("MoveShopSystem InitShopItemList [" + e + "] 오류가 발생하였습니다.");
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
        shopSaleRequestUI.SetActive(false);
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

    // 상점UI가 SetActive True 상태에서 구매 버튼을 눌렀을 때
    public void ClickShopBuyButton()
    {
        shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }

    // 상점UI가 SetActive True 상태에서 판매 버튼을 눌렀을 때
    public void ClickShopSaleButton()
    {
        // 상점 Canvas가 오픈될 때 판매UI에 있는 인벤토리 슬롯 리스트를 인벤토리 아이템 리스트로 셋팅한다. 
        InventorySystem.instance.InitShopSaleInventorySlots(ref saleInventoryEquipmentSlot, ref saleInventorySlots);
        // 인벤토리 리스트를 순회하며 상점 판매 아이템 가격을 설정한다.
        SetSalePriceSaleSlots();
        
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

    public void ClickBuyRequestYesButton()
    {
        // [상점에서 선택한 슬롯의 아이템을 구매한다.] // 선택지: [구매성공/ 구매실패]
        // 1. 선택한 상점 슬롯의 아이템을 인벤토리에 1개 추가한다.
        // 2. 플레이어가 소지하고 있는 코인에 선택한 상점 슬롯의 아이템의 가격(코인)을 감소시킨다.
        // selectBuyShopItem
        if (selectBuyShopItem == null)
        {
            print($"[장시진] 상점에서 선택한 슬롯이 존재하지 않습니다. [오류]");
            shopBuyRequestUI.SetActive(false);
            return;
        }

        foreach (var shopItemSlot in shopItemSlots)
        {
            if (shopItemSlot.item.itemName == selectBuyShopItem.item.itemName)
            {
                // 1.상점에서 판매하는 아이템이 0보다 클 경우(구매할 수 있는 조건) + 2.플레이어가 소지하고 있는 코인이 선택한 상점 슬롯의 아이템의 가격(코인)보다 크거나 같을 때 (구매할 수 있는 조건)
                if ((shopItemSlot.itemCount > 0) &&
                    (InventorySystem.instance.playerCoinCount >= selectBuyShopItem.buyItemPrice))
                {
                    // 상점에서 해당 아이템의 판매 개수를 감소한다. 이후 상점 아이템 개수 텍스트를 갱신시킨다.
                    shopItemSlot.SetSlotCount(-1);

                    // 해당 아이템을 인벤토리에 추가한다. (1개 구매)
                    InventorySystem.instance.AcquireItem(selectBuyShopItem.item, 1);

                    // 플레이어가 소지중인 코인의 개수를 아이템의 가격만큼 감소한다.
                    InventorySystem.instance.SetPlayerCoinCount(-(selectBuyShopItem.buyItemPrice));

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

                selectBuyShopItem = null;
                break;
            }
        }
    }

    public void ClickSaleRequestYesButton()
    {
        if (selectSaleShopItem == null)
        {
            print($"[장시진] 상점에서 선택한 슬롯이 존재하지 않습니다. [오류]");
            shopSaleRequestUI.SetActive(false);
            return;
        }

        foreach (var itemDB in shopItemList)
        {
            // 상점에서 판매하는 아이템DB와 판매하려는 아이템의 이름이 같을 때 
            // [조건]
            // 1. 소지하고 있는 아이템의 개수가 0보다 큰 경우(판매할 수 있는 조건) && 2. 상점 아이템DB 판매 가격이 0보다 큰 경우
            // [처리]
            // 1. 선택한 인벤토리 아이템의 개수를 1개 감소한다. (상점 인벤토리 처리, 일반 인벤토리 처리)
            // 2. 판매한 아이템의 가격을 얻는다. (코인 증가)
            // 3. 선택한 인벤토리 아이템의 개수가 0이면 슬롯 clear(초기화)를 진행한다. (Slot.cs 참고)
            // 예외) 위의 조건에 맞지 않으면 판매 불가능 UI를 출력한다.
            if ((itemDB.item.itemName == selectSaleShopItem.item.itemName) &&
                (selectSaleShopItem.itemCount > 0) && (itemDB.saleItemPrice > 0))
            {
                // 선택한 아이템 개수를 감소 시킨다.
                selectSaleShopItem.SetSlotCount(-1);

                InventorySystem.instance.SetPlayerCoinCount(itemDB.saleItemPrice);
                
                InventorySystem.instance.FindSetCountInventorySlotItem(itemDB.item.itemName, -1);
                
                // 상점 Canvas가 오픈될 때 판매UI에 있는 인벤토리 슬롯 리스트를 인벤토리 아이템 리스트로 셋팅한다. 
                // InventorySystem.Instance.InitShopSaleInventorySlots(ref saleInventoryEquipmentSlot, ref saleInventorySlots);            

                // 상점 판매UI에서 선택한 슬롯의 정보를 초기화한다.
                selectSaleShopItem = null;
                // 활성화된 RequestUI를 비활성화 한다.
                shopSaleRequestUI.SetActive(false);

                return;
            }
        }

        // [item not find] - 상점DB에 등록하지 않았을 경우 판매할 수 없는 아이템으로 판단한다.
        // 상점 판매UI에서 선택한 슬롯의 정보를 초기화한다.
        selectSaleShopItem = null;
        // 활성화된 RequestUI를 비활성화 한다.
        shopSaleRequestUI.SetActive(false);
        // 활성화된 RequestFailUI를 활성화 한다.
        shopSaleRequestFailUI.SetActive(true);
        return;
    }

    public void ClickBuyRequestNoButton()
    {
        // 상점에서 선택한 슬롯의 정보를 초기화한다. 
        selectBuyShopItem = null;

        // 활성화된 RequestUI를 비활성화 한다.
        shopBuyRequestUI.SetActive(false);
    }

    public void ClickSaleRequestNoButton()
    {
        // 상점에서 선택한 슬롯의 정보를 초기화한다. 
        selectSaleShopItem = null;

        // 활성화된 RequestUI를 비활성화 한다.
        shopSaleRequestUI.SetActive(false);
    }

    // 아이템 최종 구매 확인 UI 열기
    public void OpenRequestBuyUI(Item slotItem)
    {
        shopBuyRequestUI.SetActive(true);

        foreach (var shopItem in shopItemList)
        {
            // 상점 아이템 DB의 item과 마우스로 선택한 아이템 슬롯의 DB가 같으면 
            if (shopItem.item == slotItem)
            {
                // 현재 선택한 슬롯의 아이템을 selectBuyShopItem 변수에 담는다.
                selectBuyShopItem = shopItem;

                return;
            }
        }
    }

    // 아이템 최종 판매 확인 UI 열기
    public void OpenRequestSaleUI(Item slotItemDB)
    {
        shopSaleRequestUI.SetActive(true);

        // [key point: 각각의 아이템은 1개의 슬롯에만 존재한다.]
        // 1. 장착중인 장비 슬롯의 아이템과 마우스로 선택한 아이템이 일치할 때
        if (saleInventoryEquipmentSlot.item == slotItemDB)
        {
            selectSaleShopItem = saleInventoryEquipmentSlot;
            return;
        }

        // 2. 일반 인벤토리 슬롯의 아이템 중 마우스로 선택한 아이템이 일치할 때
        foreach (var saleSlot in saleInventorySlots)
        {
            // 상점 아이템 DB의 item과 마우스로 선택한 인벤토리 아이템 슬롯의 DB가 같으면 
            if (saleSlot.item == slotItemDB)
            {
                selectSaleShopItem = saleSlot;
                return;
            }
        }
    }

    public void OpenShopCanvas()
    {
        shopCanvas.gameObject.SetActive(true);
        playerCoinUI.SetActive(true);
    }

    // 인벤토리 슬롯들의 판매 아이템 가격을 설정한다.
    private void SetSalePriceSaleSlots()
    {
        // 인벤토리 장비 슬롯의 아이템 - 판매 가격 설정
        foreach (var shopItem in shopItemList)
        {
            if (saleInventoryEquipmentSlot.item == shopItem.item)
            {
                saleInventoryEquipmentSlot.itemSalePrice = shopItem.saleItemPrice;
                break;
            }
        }

        // 인벤토리 일반 슬롯 리스트 - 현재 소지중인 아이템의 각 슬롯의 아이템 판매 가격 설정
        foreach (var saleInventorySlot in saleInventorySlots)
        {
            saleInventorySlot.itemSalePrice = 0;

            foreach (var shopItem in shopItemList)
            {
                if (saleInventorySlot.item == shopItem.item)
                {
                    saleInventorySlot.itemSalePrice = shopItem.saleItemPrice;
                    break;
                }
            }
        }
    }
}