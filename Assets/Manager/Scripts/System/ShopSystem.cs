using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    // [프리팹 셋팅]
    [Header("상점 캔버스")] 
    [SerializeField] private GameObject _shopCanvas;

    [Header("구매 UI")] 
    [SerializeField] private GameObject _shopBuyUI;

    [Header("판매 UI")] 
    [SerializeField] private GameObject _shopSaleUI;

    [Header("상점 슬롯 설정")] 
    [SerializeField] private GameObject _shopSlotsParent;

    [Header("상점 인벤토리 슬롯 설정")] 
    [SerializeField] private GameObject _shopInventoryEquipmentSlot;
    [SerializeField] private GameObject _shopInventorySlotsParent;

    [Header("상점 버튼 설정")] 
    [SerializeField] private GameObject _shopBuyButton;
    [SerializeField] private GameObject _shopSaleButton;

    [Header("상점 뒤로가기 버튼 설정")] 
    [SerializeField] private GameObject _shopBuyBackButton;
    [SerializeField] private GameObject _shopSaleBackButton;

    [Header("상점 구매 확인 UI 설정")] 
    [SerializeField] private GameObject _shopBuyRequestUI;
    [SerializeField] private GameObject _shopBuyRequestYesButton;
    [SerializeField] private GameObject _shopBuyRequestNoButton;
    [SerializeField] private GameObject _shopBuyRequestFailUI;

    [Header("상점 판매 확인 UI 설정")] 
    [SerializeField] private GameObject _shopSaleRequestUI;
    [SerializeField] private GameObject _shopSaleRequestFailUI;
    [SerializeField] private GameObject _shopSaleRequestSuccessUI;
    [SerializeField] private TMP_Text _shopSaleRequestSuccessText;

    [Header("플레이어 코인 UI 설정")] 
    [SerializeField] private GameObject _playerCoinUI;

    [Header("일반상점 구매/판매 아이템DB 리스트")] 
    [SerializeField] private ShopItem[] _shopItemList;

    // [상점 구매UI]
    // 상점 아이템 리스트들을 슬롯에 할당한다.
    private ShopSlot[] _shopItemSlots;

    // 상점 아이템(슬롯)을 마우스로 선택하면 상점 아이템 리스트 배열의 어느 아이템인지 확인하는 변수
    private ShopItem _selectBuyShopItem;

    // [상점 판매UI]
    private SaleSlot _saleInventoryEquipmentSlot;
    private SaleSlot[] _saleInventorySlots;

    // 상점 아이템(슬롯)을 마우스로 선택하면 상점 아이템 리스트 배열의 어느 아이템인지 확인하는 변수
    private SaleSlot _selectSaleShopItem; // 마우스로 클릭하여 선택한 SaleSlot의 ref(원본)을 가져온다.

    // [상점 구매/판매 버튼 이미지]
    private Image _shopBuyButtonImage;
    private Image _shopSaleButtonImage;

    #region Shop System 싱글톤 설정
    public static ShopSystem instance;
    private void Awake()
    {
        // Game Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // 이미 Shop System이 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void Start()
    {
        _shopItemSlots = _shopSlotsParent.GetComponentsInChildren<ShopSlot>();

        _saleInventoryEquipmentSlot = _shopInventoryEquipmentSlot.GetComponent<SaleSlot>();
        _saleInventorySlots = _shopInventorySlotsParent.GetComponentsInChildren<SaleSlot>();

        Init();
    }

    private void Init()
    {
        InitShopItemList();
        InitSetButton();
        InitSetUI();
    }

    // [초기 상점 아이템 정보 셋팅]
    private void InitShopItemList()
    {
        try
        {
            for (int i = 0; i < _shopItemSlots.Length; i++)
            {
                if (_shopItemSlots[i].item == null) // 아이템 슬롯이 비어있을 때만 수행한다.
                {
                    _shopItemSlots[i].SetItem(_shopItemList[i]);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("ShopSystem InitShopItemList [" + e + "] 오류가 발생하였습니다.");
            throw;
        }
    }

    // [초기 상점 구매창/판매창 버튼 정보 셋팅]
    private void InitSetButton()
    {
        _shopBuyButtonImage = _shopBuyButton.GetComponent<Image>();
        _shopSaleButtonImage = _shopSaleButton.GetComponent<Image>();

        // 상점을 켰을 때 구매창이 먼저 활성화 된다.
        _shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        _shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }
    
    private void InitSetUI()
    {
        _shopBuyRequestUI.SetActive(false);
        _shopSaleRequestUI.SetActive(false);
    }
    
    public void ChangeBgImageAlphaSlots(int alpha)
    {
        // 모든 슬롯의 흰색 테두리 배경의 알파값을 0으로 설정한다.
        foreach (var slot in _shopItemSlots)
        {
            Color color = slot.GetComponent<Image>().color;
            color.a = alpha;
            slot.GetComponent<Image>().color = color;
        }
    }

    // 상점UI가 SetActive True 상태에서 구매 버튼을 눌렀을 때
    public void ClickShopBuyButton()
    {
        _shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        _shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }

    // 상점UI가 SetActive True 상태에서 판매 버튼을 눌렀을 때
    public void ClickShopSaleButton()
    {
        // 상점 Canvas가 오픈될 때 판매UI에 있는 인벤토리 슬롯 리스트를 인벤토리 아이템 리스트로 셋팅한다. 
        InventorySystem.instance.InitShopSaleInventorySlots(ref _saleInventoryEquipmentSlot, ref _saleInventorySlots);
        // 인벤토리 리스트를 순회하며 상점 판매 아이템 가격을 설정한다.
        SetSalePriceSaleSlots();
        
        _shopBuyButtonImage.color = new Color(1, 1, 1, 0);
        _shopSaleButtonImage.color = new Color(1, 1, 1, 1);
    }

    public void ClickCloseShopButton()
    {
        // 상점 시스템을 종료할 때 상점 구매 목록이 보이도록 설정 후 종료한다. 
        _shopBuyButtonImage.color = new Color(1, 1, 1, 1);
        _shopSaleButtonImage.color = new Color(1, 1, 1, 0);
    }

    public void ClickBuyRequestYesButton()
    {
        // [상점에서 선택한 슬롯의 아이템을 구매한다.] // 선택지: [구매성공/ 구매실패]
        // 1. 선택한 상점 슬롯의 아이템을 인벤토리에 1개 추가한다.
        // 2. 플레이어가 소지하고 있는 코인에 선택한 상점 슬롯의 아이템의 가격(코인)을 감소시킨다.
        
        if (_selectBuyShopItem == null)
        {
            Debug.Log($"[장시진] 상점에서 선택한 슬롯이 존재하지 않습니다. [오류]");
            _shopBuyRequestUI.SetActive(false);
            return;
        }

        foreach (var shopItemSlot in _shopItemSlots)
        {
            if (shopItemSlot.item.itemName == _selectBuyShopItem.item.itemName)
            {
                // 1.상점에서 판매하는 아이템이 0보다 클 경우(구매할 수 있는 조건)
                // 2.플레이어가 소지하고 있는 코인이 선택한 상점 슬롯의 아이템의 가격(코인)보다 크거나 같을 때 (구매할 수 있는 조건)
                if ((shopItemSlot.itemCount > 0) &&
                    (InventorySystem.instance.playerCoinCount >= _selectBuyShopItem.buyItemPrice))
                {
                    // 상점에서 해당 아이템의 판매 개수를 감소한다. 이후 상점 아이템 개수 텍스트를 갱신시킨다.
                    shopItemSlot.SetSlotCount(-1);

                    // 해당 아이템을 인벤토리에 추가한다. (1개 구매)
                    InventorySystem.instance.AcquireItem(_selectBuyShopItem.item, 1);

                    // 플레이어가 소지중인 코인의 개수를 아이템의 가격만큼 감소한다.
                    InventorySystem.instance.SetPlayerCoinCount(-(_selectBuyShopItem.buyItemPrice));

                    // 플레이어 UI (코인의 개수 출력 이미지 텍스트)를 갱신한다. 
                    PlayerUI playerUI = GameManager.instance.PlayerUI.GetComponent<PlayerUI>();
                    playerUI.UpdatePlayerCoinCountUI();

                    // 활성화된 RequestUI를 비활성화 한다.
                    _shopBuyRequestUI.SetActive(false);
                }
                else
                {
                    // 1.아이템 개수 소진으로 아이템 구매 불가능. or 2.플레이어 소지 코인의 개수가 부족하여 아이템 구매 불가능.

                    // 활성화된 RequestUI를 비활성화 한다.
                    _shopBuyRequestUI.SetActive(false);

                    // 활성화된 RequestFailUI를 활성화 한다.
                    _shopBuyRequestFailUI.SetActive(true);
                }

                // 마우스로 선택한 아이템 정보 초기화
                _selectBuyShopItem = null;
                break;
            }
        }
    }

    public void ClickSaleRequestYesButton()
    {
        if (_selectSaleShopItem == null)
        {
            Debug.Log($"[장시진] 상점에서 선택한 슬롯이 존재하지 않습니다. [오류]");
            _shopSaleRequestUI.SetActive(false);
            return;
        }

        foreach (var itemDB in _shopItemList)
        {
            // 상점에서 판매하는 아이템DB리스트와 판매하려는 아이템의 이름이 같을 때 
            // [조건]
            // 1. 소지하고 있는 아이템의 개수가 0보다 큰 경우(판매할 수 있는 조건) && 2. 상점 아이템DB 판매 가격이 0보다 큰 경우
            // [처리]
            // 1. 선택한 인벤토리 아이템의 개수를 1개 감소한다. (상점 인벤토리 처리, 일반 인벤토리 처리)
            // 2. 판매한 아이템의 가격을 얻는다. (코인 증가)
            // 3. 선택한 인벤토리 아이템의 개수가 0이면 슬롯 clear(초기화)를 진행한다. (Slot.cs 참고)
            // 예외) 위의 조건에 맞지 않으면 판매 불가능 UI를 출력한다.
            if ((itemDB.item.itemName == _selectSaleShopItem.item.itemName) &&
                (_selectSaleShopItem.itemCount > 0) && (itemDB.saleItemPrice > 0))
            {
                // 선택한 아이템 개수를 감소 시킨다.
                _selectSaleShopItem.SetSlotCount(-1);
                
                // 판매 가격에 대한 코인 획득, 인벤토리에 있는 아이템을 1개 감소
                InventorySystem.instance.SetPlayerCoinCount(itemDB.saleItemPrice);
                InventorySystem.instance.FindSetCountInventorySlotItem(itemDB.item.itemName, -1);
                
                // 상점 판매UI에서 선택한 슬롯의 정보를 초기화한다.
                _selectSaleShopItem = null;
                // 활성화된 RequestUI를 비활성화 한다.
                _shopSaleRequestUI.SetActive(false);
                
                // 어떤 아이템을 판매하였는지 텍스트 문구를 출력한다.
                _shopSaleRequestSuccessText.text = itemDB.item.itemName + $" 아이템을 판매하였습니다.";
                // 판매 완료 UI 출력
                _shopSaleRequestSuccessUI.SetActive(true);

                return;
            }
        }

        // [item not find] - 상점DB에 등록하지 않았을 경우 판매할 수 없는 아이템으로 판단한다.
        // 상점 판매UI에서 선택한 슬롯의 정보를 초기화한다.
        _selectSaleShopItem = null;
        // 활성화된 RequestUI를 비활성화 한다.
        _shopSaleRequestUI.SetActive(false);
        // 활성화된 RequestFailUI를 활성화 한다.
        _shopSaleRequestFailUI.SetActive(true);
        return;
    }

    public void ClickBuyRequestNoButton()
    {
        // 상점에서 선택한 슬롯의 정보를 초기화한다. 
        _selectBuyShopItem = null;

        // 활성화된 RequestUI를 비활성화 한다.
        _shopBuyRequestUI.SetActive(false);
    }

    public void ClickSaleRequestNoButton()
    {
        // 상점에서 선택한 슬롯의 정보를 초기화한다. 
        _selectSaleShopItem = null;

        // 활성화된 RequestUI를 비활성화 한다.
        _shopSaleRequestUI.SetActive(false);
    }

    // 아이템 최종 구매 확인 UI 열기
    public void OpenRequestBuyUI(Item slotItem)
    {
        _shopBuyRequestUI.SetActive(true);

        foreach (var shopItem in _shopItemList)
        {
            // 상점 아이템 DB의 item과 마우스로 선택한 아이템 슬롯의 DB가 같으면 
            if (shopItem.item == slotItem)
            {
                // 현재 선택한 슬롯의 아이템을 selectBuyShopItem 변수에 담는다.
                _selectBuyShopItem = shopItem;

                return;
            }
        }
    }

    // 아이템 최종 판매 확인 UI 열기
    public void OpenRequestSaleUI(Item slotItemDB)
    {
        _shopSaleRequestUI.SetActive(true);

        // [key point: 각각의 아이템은 1개의 슬롯에만 존재한다.]
        // 1. 장착중인 장비 슬롯의 아이템과 마우스로 선택한 아이템이 일치할 때
        if (_saleInventoryEquipmentSlot.item == slotItemDB)
        {
            _selectSaleShopItem = _saleInventoryEquipmentSlot;
            return;
        }

        // 2. 일반 인벤토리 슬롯의 아이템 중 마우스로 선택한 아이템이 일치할 때
        foreach (var saleSlot in _saleInventorySlots)
        {
            // 상점 아이템 DB의 item과 마우스로 선택한 인벤토리 아이템 슬롯의 DB가 같으면 
            if (saleSlot.item == slotItemDB)
            {
                _selectSaleShopItem = saleSlot;
                return;
            }
        }
    }

    public void OpenShopCanvas()
    {
        _shopCanvas.gameObject.SetActive(true);
        _playerCoinUI.SetActive(true);
    }

    // 인벤토리 슬롯들의 판매 아이템 가격을 설정한다.
    private void SetSalePriceSaleSlots()
    {
        // 인벤토리 장비 슬롯의 아이템 - 판매 가격 설정
        foreach (var shopItem in _shopItemList)
        {
            if (_saleInventoryEquipmentSlot.item == shopItem.item)
            {
                _saleInventoryEquipmentSlot.itemSalePrice = shopItem.saleItemPrice;
                break;
            }
        }

        // 인벤토리 일반 슬롯 리스트 - 현재 소지중인 아이템의 각 슬롯의 아이템 판매 가격 설정
        foreach (var saleInventorySlot in _saleInventorySlots)
        {
            saleInventorySlot.itemSalePrice = 0;

            foreach (var shopItem in _shopItemList)
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