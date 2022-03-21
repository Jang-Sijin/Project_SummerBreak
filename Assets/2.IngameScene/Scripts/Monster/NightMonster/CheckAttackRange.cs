using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckAttackRange : Node
{

    private static int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    
    private Transform _transform;
    private Transform target_transform;
    private Animator _animator;
    
    public CheckAttackRange(Transform transform, Transform t_transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        target_transform = t_transform;
    }

    public override NodeState Evaluate()
    { 
        if (Vector3.Distance(_transform.position, target_transform.position) 
          <= NightMonsterBT.attackRange)
        {
            
            _animator.SetBool("Chasting", false);
            _animator.SetBool("Attack", false);
            _animator.SetBool("Idle", true);
            _animator.SetBool("Hit", false);
            
            
            Debug.Log("범위에 있음");
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }
    
}
