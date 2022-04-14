using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class HitForTarget : Node
{
    private MonsterManager monsterManager;
    private Animator _animator;

    public HitForTarget(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        monsterManager = transform.GetComponent<MonsterManager>();
    }

    public override NodeState Evaluate()
    {
        
        //Debug.Log("[이민호]타격함2");
        monsterManager.KnockBack();
        monsterManager.TakeHit();
        
        _animator.SetBool("Chasting", false);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Idle", false);
        _animator.SetBool("Hit", true);
        monsterManager.checkHit = false;
        
        
        state = NodeState.RUNNING;
        return state;
    }
}
