using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [Header("게임플레이 키설정")]
    public KeyCode option;

    [Header("게임 옵션 오브젝트")]
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject playerCoinUI;
    
    [SerializeField] private GameObject inventoryUICanvas;
    [SerializeField] private GameObject shopUICanvas;
    [SerializeField] private GameObject dialogUICanvas;
    
    public bool showOptionUI;
    private bool isOpenOptionUI;
    private bool isOpenInventoryUI;

    private void Awake()
    {
        showOptionUI = false;
        isOpenOptionUI = false;
        isOpenInventoryUI = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(option))
        {
            if (shopUICanvas.gameObject.activeSelf != true &&
                dialogUICanvas.gameObject.activeSelf != true)
            {
                showOptionUI = !showOptionUI;
            }
        }

        if (showOptionUI)
        {
            OptionUIOpen();
        }
        else
        {
            OptionUIClose();
        }
    }

    private void OptionUIOpen()
    {
        if (isOpenOptionUI)
            return;
        else
        {
            optionUI.SetActive(true);
            playerCoinUI.SetActive(true);
            isOpenOptionUI = true;
            GameManager.instance.InGameTimeStop();

            if (isOpenInventoryUI == true)
            {
                inventoryUICanvas.SetActive(true);
                isOpenInventoryUI = false;
            }
        }
    }
    
    private void OptionUIClose()
    {
        if (!isOpenOptionUI)
            return;
        else
        {
            optionUI.SetActive(false);
            playerCoinUI.SetActive(false);
            isOpenOptionUI = false;
            GameManager.instance.InGameTimeStart();

            if (inventoryUICanvas.gameObject.activeSelf == true)
            {
                isOpenInventoryUI = true;
                inventoryUICanvas.SetActive(false);
            }
        }
    }
}