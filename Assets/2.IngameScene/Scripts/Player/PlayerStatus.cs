using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class PlayerStatus : MonoBehaviour
{
    public enum item
    {
        nothing,
        attack,
        interaction_sleep,
        interaction_quillPen
    }

    public item currentItem;
    
    [SerializeField]
    private float maxHealth = 100.0f;
    public float currentHealth;
    public float currentMaxstamina;
    public float currentStamina;
    public float walkSpeed = 4.0f;
    public float runValue = 2.0f;
    public float runSpeed;

    [SerializeField] 
    private GameObject equipmentSlotObj;

    [SerializeField] 
    private GameObject DebugModTextObj;
    [SerializeField] 
    private GameObject DebugModGlideButton;
    [SerializeField] 
    private GameObject DebugModQuaterButtton;
    [SerializeField] 
    private GameObject DebugModGetItemButton;
    [SerializeField] 
    private GameObject DebugModMoveButton;
    
    private Slot equipmentSlot;

    private PlayerUI playerUI;

    private Image[] playerHpImageArray;
    [SerializeField] 
    private Image[] playerStaminaImageArray;
    [SerializeField] 
    private Image[] playerMaxStaminaImageArray;
    
    [SerializeField]
    private bool DebugMod = false;

    private PlayerMovement _playerMovement;

    public bool playerInPeak = false;

    [SerializeField] private float resetHp;
    
    [SerializeField] private string checkToChangeEquipment = null;
    
    [SerializeField] private PlayableDirector _respawnCutScene;
    
    void Awake()
    {
        currentHealth = maxHealth;
        if (DebugMod)
        {
            currentMaxstamina = 100.0f;
            currentStamina = currentMaxstamina;
        }

        runSpeed = walkSpeed * runValue;
        currentItem = item.nothing;
    }

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        equipmentSlot = equipmentSlotObj.GetComponent<Slot>();
        playerUI = GameManager.instance.PlayerUI.GetComponent<PlayerUI>();
        playerHpImageArray = playerUI.GetHpImageArray();
        playerStaminaImageArray = playerUI.GetStaminaImageArray();
        playerMaxStaminaImageArray = playerUI.GetMaxStaminaImageArray();
    }
    
    public void DebugModActive()
    {
        if (DebugMod == false)
        {
            DebugMod = true;
            DebugModTextObj.SetActive(true);
            DebugModGlideButton.SetActive(true);
            DebugModQuaterButtton.SetActive(true);
            DebugModGetItemButton.SetActive(true);
            DebugModMoveButton.SetActive(true);
            GameManager.instance.SetTimeMultiplier(2000);
        }
        else
        {
            PlayerMovement playerMovement = this.GetComponent<PlayerMovement>();
            playerMovement.SetDebugMod(true);
            DebugMod = false;
            DebugModTextObj.SetActive(false);
            DebugModGlideButton.SetActive(false);
            DebugModQuaterButtton.SetActive(false);
            DebugModGetItemButton.SetActive(false);
            DebugModMoveButton.SetActive(false);
            GameManager.instance.SetTimeMultiplier(540);
        }
    }

    
    private void Update()
    {
        /*
        if (currentHealth <= 0.0f)
        {
        }
        */


        if (equipmentSlot.item == null)
        {
            currentItem = item.nothing;
            checkToChangeEquipment = null;
        }
        else
        {
            if (equipmentSlot.item.itemName != checkToChangeEquipment)
            {
                if (equipmentSlot.item.itemName == "작은 검")
                {
                    Debug.Log("[이민호] 작은 검 장착");
                    currentItem = item.attack;
                    checkToChangeEquipment = equipmentSlot.item.itemName;
                }
                else if (equipmentSlot.item.itemName == "폭신침낭")
                {
                    Debug.Log("[이민호] 폭신침낭 장착");
                    currentItem = item.interaction_sleep;
                    checkToChangeEquipment = equipmentSlot.item.itemName;
                }
                else if (equipmentSlot.item.itemName == "검은깃펜")
                {
                    Debug.Log("[이민호] 검은 깃펫 장착");
                    currentItem = item.interaction_quillPen;
                    checkToChangeEquipment = equipmentSlot.item.itemName;
                }
                else
                {
                    currentItem = item.nothing;
                    checkToChangeEquipment = null;
                }
            }
        }

        for (int i = 0; i < 10; ++i)
        {
            if (i <= (int)(currentHealth / 10) - 1)
            {
                playerHpImageArray[i].color = new Color(255, 255, 255, 255);
            }
            else
            {
                playerHpImageArray[i].color = new Color(255, 255, 255, 0);
            }
        }
        
        for (int i = 0; i < 13; ++i)
        {
            if (i <= (int)(currentMaxstamina / 10) - 1)
            {
                playerMaxStaminaImageArray[i].color = new Color(255, 255, 255, 255);
                playerStaminaImageArray[i].color = new Color(255, 255, 255, 255);
            }
            else
            {
                playerMaxStaminaImageArray[i].color = new Color(255, 255, 255, 0);
                playerStaminaImageArray[i].color = new Color(255, 255, 255, 0);
            }
        }

        for (int i = 0; i < (int) currentMaxstamina / 10; ++i)
        {
            playerStaminaImageArray[i].fillAmount = (currentStamina - (i * 10.0f)) * 0.1f;
        }
        
    }

    public void TakeStamina(float stamina)
    {
        //Debug.Log("[이민호]현재 스태미나 : " + currentStamina);
        currentStamina -= stamina;
        //Debug.Log("[이민호]사용 후 스태미나 : " + currentStamina);
    }

    public void HealthStamina(float stamina)
    {
        if (currentStamina < currentMaxstamina)
        {
            currentStamina += stamina;
        }
    }

    public void HealHealth(float value)
    {
        if (currentHealth + value >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += value;
        }
        
    }   
    
    public void HitHealth(float damageValue)
    {
        currentHealth -= damageValue;
    }
    
    public void ReSetCurHealth()
    {
        currentHealth = resetHp;
    }
    public void HealStamina()
    {
        if (currentStamina >= currentMaxstamina)
        {
            currentStamina = currentMaxstamina;
        }
        else
        {
            currentStamina += 10.0f;
        }
    }

    public void HealMaxStamina()
    {

        if (currentMaxstamina < 130)
        {
            currentMaxstamina += 10.0f;
        }
    }
    //Getter
    public float GetCurHealth()
    {
        return currentHealth;
    }

    public bool GetDebugMod()
    {
        return DebugMod;
    }
    public float GetCurStamina()
    {
        return currentStamina;
    }

    public float GetMaxStamina()
    {
        return currentMaxstamina;
    }
    //Setter
    public void SetCurHealth(float value)
    {
        currentHealth += value;
    }
    public void SetCurStamina(float value)
    {
        currentStamina += value;
    }
    public void SetMaxStamina(float value)
    {
        currentMaxstamina += value;
    }
}
