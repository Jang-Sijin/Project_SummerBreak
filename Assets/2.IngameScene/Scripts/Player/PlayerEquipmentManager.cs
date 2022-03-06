using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private PlayerStatus m_playerStatus;
    private PlayerMovement m_player;
    
    public GameObject equipmentAttack;

    private CapsuleCollider colliderAttack;
    
    private void Awake()
    {
        m_player = GetComponent<PlayerMovement>();
        m_playerStatus = GetComponent<PlayerStatus>();
        
        colliderAttack = equipmentAttack.GetComponent<CapsuleCollider>();

        colliderAttack.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_playerStatus.currentItem == PlayerStatus.item.attack)
        {
            equipmentAttack.SetActive(true);
        }
        else
        {
            equipmentAttack.SetActive(false);
        }
    }

    public void AttackStart()
    {
        colliderAttack.enabled = true;
    }

    public void AttackEnd()
    {
        colliderAttack.enabled = false;
    }

    public void AttackCancel()
    {
        m_player.currentState = PlayerMovement.playerState.Ground_idleState;
    }
    
}