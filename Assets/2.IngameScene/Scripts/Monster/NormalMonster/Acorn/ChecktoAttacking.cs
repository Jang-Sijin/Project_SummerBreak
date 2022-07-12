using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ChecktoAttacking : Node
{
    private Animator _animator;

    public ChecktoAttacking(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            Debug.Log("[이민호] 공격중");
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;

    }
}
