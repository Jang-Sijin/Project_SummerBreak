using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;


public class AtttackToTarget : Node
{
    private Animator _animator;
    private PlayerMovement playerMovement;
    private Rigidbody _rigidbody;
    private MonsterManager _monsterManager;
    
    
    private float _attackTime = 0.5f;
    private float _attackCounter = 0f;
    
    public AtttackToTarget(Transform transform)
    {
        _monsterManager = transform.GetComponent<MonsterManager>();
        _animator = transform.GetComponent<Animator>();
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    public override NodeState Evaluate()
    {
        Transform _lastTarget = (Transform)GetData("target");
        playerMovement = _lastTarget.GetComponent<PlayerMovement>();
        _attackCounter += Time.deltaTime;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
        float damageValue = _monsterManager.curMonsterType == MonsterManager.monsterType.green_slimyee ?  
                NightMonsterBT.damageValue : SlimyeeBT.damageValue;
        if (_attackCounter >= _attackTime)
        {
            //Debug.Log("[이민호] 몬스터가 공격함");
            if (_lastTarget.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (_monsterManager.curMonsterType == MonsterManager.monsterType.green_slimyee ||
                    _monsterManager.curMonsterType == MonsterManager.monsterType.red_slimyee ||
                    _monsterManager.curMonsterType == MonsterManager.monsterType.nightMonster_slimyee)
                {
                    playerMovement.HitStart(damageValue, _rigidbody);
                }
            }

            _animator.SetBool("Attack", true);
            _animator.SetBool("Idle", false);
            bool targetIsDead = false;
            if (!targetIsDead)
            {
                _attackCounter = 0f;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
    
}
