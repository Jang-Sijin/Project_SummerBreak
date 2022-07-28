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

    [SerializeField] 
    private GameObject _gameObject;
    
    private void Awake()
    {
        m_player = GetComponent<PlayerMovement>();
        m_playerStatus = GetComponent<PlayerStatus>();
        
        colliderAttack = equipmentAttack.GetComponent<CapsuleCollider>();

        colliderAttack.enabled = false;
        _gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (m_playerStatus.currentItem != PlayerStatus.item.nothing)
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
        else
        {
            equipmentAttack.SetActive(false);
        }
    }

    public void AttackStart()
    {
        SoundManager.Instance.PlaySFX(0);
        colliderAttack.enabled = true;
    }

    public void AttackEnd()
    {
        colliderAttack.enabled = false;
    }

    public void AttackAwake()
    {
        m_player.attacked = true;
        _gameObject.SetActive(true);
    }

    public void AttackCancel()
    {
        m_player.currentState = PlayerMovement.playerState.Ground_idleState;
        m_player.attacked = false;
        _gameObject.SetActive(false);
    }
    
}
