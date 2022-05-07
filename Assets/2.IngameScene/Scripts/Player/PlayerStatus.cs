using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField]
    private float currentHealth;
    public float currentMaxstamina;
    public float currentStamina;
    public float walkSpeed = 4.0f;
    public float runValue = 2.0f;
    public float runSpeed;

    [SerializeField] 
    private GameObject equipmentSlotObj;

    private Slot equipmentSlot;

    private PlayerUI playerUI;

    private Image[] playerHpImageArray;

    private Image[] playerStaminaImageArray;

    private Image[] playerMaxStaminaImageArray;
    
    public bool DebugMod = false;
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
        equipmentSlot = equipmentSlotObj.GetComponent<Slot>();
        playerUI = GameManager.instance.PlayerUI.GetComponent<PlayerUI>();
        playerHpImageArray = playerUI.GetHpImageArray();
        playerStaminaImageArray = playerUI.GetStaminaImageArray();
        playerMaxStaminaImageArray = playerUI.GetMaxStaminaImageArray();
    }

    private void Update()
    {
        
        if (equipmentSlot.item != null)
        {
            if (equipmentSlot.item.itemName == "소드")
            {
                //currentItem = item.attack;
                //currentItem = item.interaction_sleep;
                currentItem = item.interaction_quillPen;
            }
        }
        else
        {
            currentItem = item.nothing;
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
        
        for (int i = 0; i < 10; ++i)
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

    public void UpgradeCurMaxStamina(float stamina)
    {
        currentMaxstamina += stamina;
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
        currentHealth = maxHealth / 2;
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
        currentMaxstamina += 10.0f;
    }
    //Getter
    public float GetCurHealth()
    {
        return currentHealth;
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
