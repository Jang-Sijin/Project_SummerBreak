using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AttackToTarget_Acorn : Node
{
    private Animator _animator;
    private PlayerMovement playerMovement;
    private Rigidbody _rigidbody;
    private MonsterManager _monsterManager;

    private float _attackTime = 0.5f;
    private float _attackCounter = 0.0f;

    public AttackToTarget_Acorn(Transform transform)
    {
        _monsterManager = transform.GetComponent<MonsterManager>();
        _animator = transform.GetComponent<Animator>();
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    public override NodeState Evaluate()
    {
        Transform _lastTarget = (Transform) GetData("target");
        playerMovement = _lastTarget.GetComponent<PlayerMovement>();
        _attackCounter += Time.deltaTime;
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", true);
        float damageValue;
        if (_monsterManager.curMonsterType == MonsterManager.monsterType.nightMonster)
        {
            damageValue = NightMonsterBT.damageValue;
        }
        else if (_monsterManager.curMonsterType == MonsterManager.monsterType.acorn)
        {
            damageValue = AcornBT.damageValue;
        }
        else
        {
            damageValue = SlimyeeBT.damageValue;
        }
        
        
        state = NodeState.RUNNING;
        return state;
    }

}
