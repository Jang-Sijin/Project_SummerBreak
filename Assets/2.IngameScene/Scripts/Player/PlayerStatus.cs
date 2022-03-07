using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    
    
    public enum item
    {
        nothing,
        attack,
        interaction
    }

    public item currentItem;
    
    public string name;
    public float maxHealth = 100.0f;
    public float currentHealth;

    public float currentMaxstamina;
    public float currentStamina;
    
    public float walkSpeed = 4.0f;
    public float runValue = 2.0f;
    public float runSpeed;

    void Awake()
    {
        currentHealth = maxHealth;
        currentMaxstamina = 100.0f;
        currentStamina = currentMaxstamina;
        runSpeed = walkSpeed * runValue;
        currentItem = item.nothing;
    }

    public void UpgradeCurMaxStamina(float stamina)
    {
        currentMaxstamina += stamina;
    }
    
    public void TakeStamina(float stamina)
    {
        Debug.Log("현재 스태미나 : " + currentStamina);
        currentStamina -= stamina;
        Debug.Log("사용 후 스태미나 : " + currentStamina);
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
}
