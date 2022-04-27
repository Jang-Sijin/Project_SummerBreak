using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAttackRange : Node
{

    private Transform _transform;
    private Transform target_transform;
    private Animator _animator;
    private MonsterManager _monsterManager;
    
    public CheckAttackRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _monsterManager = transform.GetComponent<MonsterManager>();
    }

    public override NodeState Evaluate()
    {
        target_transform = (Transform)GetData("target");
        if (target_transform == null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        float attackRange = _monsterManager.curMonsterType == MonsterManager.monsterType.nightMonster
            ? NightMonsterBT.attackRange
            : SlimyeeBT.attackRange;
        
        if (Vector3.Distance(_transform.position, target_transform.position) 
          <= attackRange)
        {
            
            _animator.SetBool("Chasting", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Idle", true);
            _animator.SetBool("Hit", false);
            
            
            //Debug.Log("[이민호] 범위에 있음");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }
    
}
